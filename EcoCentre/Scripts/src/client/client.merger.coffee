client = require('client');

class ClientViewModel extends Knockback.ViewModel
  constructor:(model)->
    super model
    this.model = model

    this.categories = new (require('client')).CategoriesModel()
    this.categories.fetch()
    this.fullName = ko.computed this.computeFullName

    this.invoices = kb.collectionObservable(this.model.Invoices)

    this.showContactWarning = ko.computed this.computeShowContactWarning
    this.showUnverifiedWarning = ko.computed this.computeShowUnverifiedWarning
    this.showComment = ko.computed this.computeShowComment

  computeFullName:()=>this.FirstName() + ' ' + this.LastName()
  computeShowUnverifiedWarning:()=>
    !this.Verified()

  computeShowComment:()=>
    @Comments()!= null && @Comments() != ''

  computeShowContactWarning:()=>
    email = this.Email()
    phone = this.PhoneNumber()
    email == null || email.length < 1 || phone == null || phone.length < 0;

  showFirstNameLastName:()=>
    this.categories.showFirstNameLastName(this.get('Category'))

class ClientMergerModel extends Backbone.Model
  constructor:()->
    this.url = 'client/merge'
    super
      MergeDest: null
      MergeSources: []

  selectMergeDest:(client)=>
    this.set
      MergeDest: client.Id()

  addMergeSource:(client)=>
    curMergeSources = this.get('MergeSources')
    if !~curMergeSources.indexOf(client.Id())
      curMergeSources.push client.Id()
      client.client_merger_clientWasPicked true
      return true
    return false

  removeMergeSource:(client)=>
    curMergeSources = this.get('MergeSources')
    index = curMergeSources.indexOf client.Id()
    if ~index
      curMergeSources.splice index, 1
      client.client_merger_clientWasPicked false
    return index

  save:()=>
    mergeSourcesString = this.get('MergeSources').join(',')
    this.set
      MergeSourcesStr: mergeSourcesString
    super.save

  resetMergeDest:(client)=>
    if client.Id() == this.get('MergeDest')
      this.set
        MergeDest: null
      return true
    return false

class ViewModel extends Knockback.ViewModel
  constructor:(model, clientsList)->
    this.model = model
    super model
    this.errors = ko.observableArray []
    this.clientsList = clientsList
    this.mergeDest = ko.observable(null)
    this.mergeSources = ko.observableArray() #new ClientsViewModel(model, clientSrcs)
    this.showDestList = ko.observable true

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
    eco.app.notifications.addNotification('msg-client-merger-run-successfully');
    eco.app.router.navigate 'client/show/'+this.model().get('MergeDest'), {trigger:true}

  onError:(m,errors,data)=>
    setTimeoutCallback = -> $("#global-loading-fade").modal('hide')
    setTimeout setTimeoutCallback, 500
    errors = $.parseJSON errors.responseText
    this.errors.removeAll()
    eco.app.notifications.removeNotification('msg-client-merger-run-successfully')
    for item in errors
      this.errors.push item

  onRemoveSrcPick : (client)=>
    index = this.model().removeMergeSource client
    if ~index
      this.mergeSources.splice index, 1
      this.removeClientNameFromHeaderElement $('#merge-sources-names'), client
  changeDest:(client)=>
    this.showDestList true
    
  addClientNameToHeaderElement:($element, client, replace)=>
    elementText = $element.text()
    clientFullName = client.LastName() + ' ' + client.FirstName()
    unless replace
      $element.text if elementText && elementText != '-' then elementText + ', ' + clientFullName else clientFullName
    else
      $element.text clientFullName

  removeClientNameFromHeaderElement:($element, client)=>
    elementText = $element.text()
    clientFullName = client.LastName() + ' ' + client.FirstName()
    removalRegExp = new RegExp('(,\\s)?' + clientFullName.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'))
    elementText = elementText.replace removalRegExp, ''
    elementText = elementText.replace /^,\s/g, ''
    elementText = '-' unless elementText
    return $element.text(elementText)
#
#    editClient:()=> eco.app.router.navigate 'invoice/new/client/edit/'+this.client().Id(),{trigger:true}
#    newClient:()=> eco.app.router.navigate 'invoice/new/client/create',{trigger:true}


class ClientMergerWorkflow
  constructor:()->
    this.formViewModel = null
    this.merger = null

  runMerger:()=>
    this.merger = new ClientMergerModel()

    clientsList = this.showClientList();
    clientsList.model.clients.bind 'add', (item)=>
#     inject the new observable into the model
      item.set
        client_merger_clientWasPicked: false


    clientsList.reset()
    return this.formViewModel = new ViewModel(this.merger,clientsList)

  showClientList:(setup=null)=>
    clientListVm = new (require 'client.list').createViewModel();
    clientListVm.mode 'pick'
    clientListVm.model.set
      noCommercial : true
    clientListVm.onDestPick = (client)=>
      this.merger.selectMergeDest client
      this.formViewModel.mergeDest new ClientViewModel client.model()
      this.formViewModel.showDestList false
      this.formViewModel.addClientNameToHeaderElement $('#merge-dest-names'), client, true
    clientListVm.onSrcPick = (client)=>
      if this.merger.addMergeSource client
        this.formViewModel.mergeSources.push client
        if this.merger.resetMergeDest client
          this.formViewModel.changeDest()
          this.formViewModel.removeClientNameFromHeaderElement $('#merge-dest-names'), client
        this.formViewModel.addClientNameToHeaderElement $('#merge-sources-names'), client

    setup(clientListVm) if setup?
    clientListVm

exports.createViewModel = ()=>
  workflow = new ClientMergerWorkflow()
  return workflow.runMerger()