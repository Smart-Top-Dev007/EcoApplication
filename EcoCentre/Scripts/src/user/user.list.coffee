common = require 'common'

class UserViewModel
  constructor:(data)->
    this.model = data;
    this.Login = data.get 'Login'
    this.Id = data.get 'Id'
    this.IsAdmin = kb.observable data, {key: 'IsAdmin', localizer: common.LocalizedStringLocalizer}
    this.IsGlobalAdmin = kb.observable data, {key: 'IsGlobalAdmin', localizer: common.LocalizedStringLocalizer}
    this.IsReadOnly = kb.observable data, {key: 'IsReadOnly', localizer: common.LocalizedStringLocalizer}
    this.IsLoginAlertEnabled = kb.observable data, {key: 'IsLoginAlertEnabled', localizer: common.LocalizedStringLocalizer}


class ViewModel

  constructor:(model)->
    this.model = model
    this.items = kb.collectionObservable this.model,{view_model:UserViewModel}

  load:()=> this.model.fetch()

  changePage:(vm)=>this.model.changePage(vm.number)

  search:()=>this.model.search()
  onNew:()=>eco.app.router.navigate 'user/new', {trigger:true}
  onEdit:(vm)=>eco.app.router.navigate 'user/edit/'+vm.Id, {trigger:true}
  onRemove:(vm)=>
    return unless confirm kb.locale_manager.get("Do you want to remove the user?")
    vm.model.destroy()



exports.createViewModel =  ()=>
  model = new (require('user')).ListModel()
  new ViewModel(model)