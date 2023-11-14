material = require('material');

class MaterialMergerModel extends Backbone.Model
  constructor:()->
    this.url = 'material/merge'
    super
      MergeDest: null
      MergeSources: []
  
  selectMergeDest:(material)=>
    this.set
      MergeDest: material.Id()
    
  addMergeSource:(material)=>
    curMergeSources = this.get('MergeSources')
    if !~curMergeSources.indexOf(material.Id())
      curMergeSources.push material.Id()
      material.material_merger_materialWasPicked true
      return true
    return false

  removeMergeSource:(material)=>
    curMergeSources = this.get('MergeSources')
    index = curMergeSources.indexOf material.Id()
    if ~index
      curMergeSources.splice index, 1
      material.material_merger_materialWasPicked false
    return index

  save:()=>
    mergeSourcesString = this.get('MergeSources').join(',')
    this.set
      MergeSourcesStr: mergeSourcesString
    super.save
    
  resetMergeDest:(material)=>
    if material.Id() == this.get('MergeDest')
      this.set
        MergeDest: null
      return true
    return false
    
class ViewModel extends Knockback.ViewModel
  constructor:(model, materials)->
    this.model = model
    super model
    this.errors = ko.observableArray []
    this.materials = materials
    this.mergeDest = ko.observable(null)
    this.mergeSources = ko.observableArray() #new MaterialsViewModel(model, materialSrcs)
    this.showDestList = ko.observable true
    this.materials.load()
        
  load:()=>this.model().fetch()
  merge:()=>
    if !this.mergeDest() || !this.mergeSources().length
      alert("Se il vous plaît, sélectionnez les deux fusionnent sources et fusionner destination de continuer.")
      return
    if !confirm("Attention! L'opération est irréversible! Etes-vous sûr de vouloir continuer?")
      return
    $("#global-loading-fade").modal('show')
    this.model().save(null,{success : this.onMerged, error:this.onError})
  
  onMerged:()=>
    setTimeoutCallback = -> $("#global-loading-fade").modal('hide')
    setTimeout setTimeoutCallback, 500
    eco.app.notifications.addNotification('msg-material-merger-run-successfully');
    eco.app.router.navigate 'material/index', {trigger:true}

  onError:(m,errors,data)=>
    setTimeoutCallback = -> $("#global-loading-fade").modal('hide')
    setTimeout setTimeoutCallback, 500
    errors = $.parseJSON errors.responseText
    this.errors.removeAll()
    eco.app.notifications.removeNotification('msg-material-merger-run-successfully')
    for item in errors
      this.errors.push item
      
  onRemoveSrcPick : (material)=>
    index = this.model().removeMergeSource material
    if ~index
      this.mergeSources.splice index, 1
      this.removeMaterialNameFromHeaderElement $('#merge-sources-names'), material
  changeDest:()=>
    this.showDestList true
    
  addMaterialNameToHeaderElement:($element, material, replace)=>
    elementText = $element.text()
    materialFullName = material.Name() + ' (' + material.Tag() + ')'
    unless replace
      $element.text if elementText && elementText != '-' then elementText + ', ' + materialFullName else materialFullName
    else
      $element.text materialFullName

  removeMaterialNameFromHeaderElement:($element, material)=>
    elementText = $element.text()
    materialFullName = material.Name() + ' (' + material.Tag() + ')'
    removalRegExp = new RegExp('(,\\s)?' + materialFullName.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'))
    elementText = elementText.replace removalRegExp, ''
    elementText = elementText.replace /^,\s/g, ''
    elementText = '-' unless elementText
    return $element.text(elementText)
#
#    editMaterial:()=> eco.app.router.navigate 'invoice/new/material/edit/'+this.material().Id(),{trigger:true}
#    newMaterial:()=> eco.app.router.navigate 'invoice/new/material/create',{trigger:true}


class MaterialMergerWorkflow
  constructor:()->
    this.formViewModel = null
    this.merger = null
    
#    redirectIfNewInvoice:()=>
#      if this.formViewModel == null
#          this.navigate 'invoice/new'
#          true
#      false

  runMerger:()=>
    this.merger = new MaterialMergerModel()
        
    materials = this.showMaterialList();
    materials.model.materials.bind 'add', (item)=>
#     inject the new observable into the model
      item.set
        material_merger_materialWasPicked: false
    
    return this.formViewModel = new ViewModel(this.merger,materials)

  showMaterialList:(setup=null)=>
    materialListVm = new (require 'material.list').createViewModel();
    materialListVm.onDestPick = (material)=>
      this.merger.selectMergeDest material
      this.formViewModel.mergeDest material
      this.formViewModel.showDestList false
      this.formViewModel.addMaterialNameToHeaderElement $('#merge-dest-names'), material, true
    materialListVm.onSrcPick = (material)=>
      if this.merger.addMergeSource material
        this.formViewModel.mergeSources.push material
        if this.merger.resetMergeDest material
          this.formViewModel.changeDest
          this.formViewModel.removeMaterialNameFromHeaderElement $('#merge-dest-names'), material
        this.formViewModel.addMaterialNameToHeaderElement $('#merge-sources-names'), material
    materialListVm

exports.createViewModel = ()=>
  workflow = new MaterialMergerWorkflow()
  return workflow.runMerger()