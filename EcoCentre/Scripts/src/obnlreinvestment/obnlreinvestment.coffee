common = require('common')

class OBNLNumberAutocompleteViewModel
  constructor:(value)->
    this.value = value

  select:(e,i)=>
    this.value i.item.label
    return false

  change:(e)=> return false

  source:( request, response )=>
    url = '/client/suggestobnl'
    data = {term: request.term}
    $.get url,data,(cat)=>response(cat)

class OBNLReinvestmentsList extends Backbone.Model
    obnlReinvestments : new Backbone.Collection()
    pageButtons : new Backbone.Collection()
    urlRoot : '/obnlreinvestment/index'
    constructor:()->
        this.obnlReinvestments.url = '/obnlreinvestment/index'
        super
            filterTerm : ''
            filterTermFrom : null
            filterTermTo : null
            filterType : 'obnlReinvestmentNo'
            filterDeleted : false
            CurrentYear : true
            sortBy : 'obnlReinvestmentDate'
            sortDir : 'desc'
            userId : ''
            page : 1
            pageCount: 1
            pageSize: null
            centerName : 'Tous'
            total: 0
        
    loadForUser:(id)=>
        this.set 'userId',id
        this.search()

    parse:(resp)=>
        this.obnlReinvestments.reset()
        this.set 'total', resp.Total
        for item in resp.OBNLReinvestments
            model = new OBNLReinvestmentModel
            model.set item
            this.obnlReinvestments.push model

        this.pageButtons.reset()
        pages = common.generatePages(resp.Page,resp.PageCount)
        this.set 'page', resp.Page
        this.set 'pageCount', resp.PageCount
        for page in pages
            this.pageButtons.push page

    search:()=>
        this.changePage(1)

    changePage:(page)=>
        this.set 'page', page
        from = this.get 'filterTermFrom'
        to = this.get 'filterTermTo'
        from = from.toString('yyyy-MM-dd') if from?
        to = to.toString('yyyy-MM-dd') if to?
        $loadingFade = $("#global-loading-fade");
        $loadingFade.modal('show');
        fetchAjax = this.fetch data:
            page : this.get 'page'
            pageSize: this.get 'pageSize'
            userId : this.get 'userId'
            sortDir : this.get 'sortDir'
            sortBy : this.get 'sortBy'
            CurrentYear : this.get 'CurrentYear'
            Deleted : this.get 'filterDeleted'
            Type : this.get 'filterType'
            Term : this.get 'filterTerm'
            TermFrom : from
            TermTo : to,
            CenterName : this.get 'centerName'
        fetchAjax.complete () =>
          setTimeoutCallback = -> $loadingFade.modal('hide')
          setTimeout setTimeoutCallback, 500
          return fetchAjax;


class OBNLReinvestmentModel extends Backbone.Model
    urlRoot : '/obnlreinvestment/index'
    idAttribute : 'Id'
    constructor:()->
        super
            OBNLReinvestmentNo : ''
            Id : ''
            CreatedAt : ''
            Comment : ''
            CreatedBy : null
            Attachments : []
            Materials : []
            Address : new Backbone.Model
                City:'c'
                CItyId:'i'
                Street:'d'
                CivicNumber:'s'
                PostalCode:'a'
            Client:
                FirstName : ''
                LastName : ''
                PhoneNumber : ''
                Address : new Backbone.Model
                    City:''
                    CItyId:''
                    Street:''
                    CivicNumber:''
                    PostalCode:''
            Center:
                Name: ''
                Url: ''
    fetchById:(id)=>
        this.fetch {data:{id:id} }
    
    url:()=> 
        return "/obnlreinvestment" if this.isNew()
        "/obnlreinvestment/index/"+this.id


class NewOBNLReinvestmentModel extends Backbone.Model
    url : 'obnlreinvestment'
    constructor:()->
        this.Client = ko.observable(null)
        super
            Id : ''
            Comment : ''
            CreatedBy: null
            ClientId : 0
            Attachments:[]
            EmployeeName : 0
            Materials : Array()
            
    selectClient:(client)=>
        this.Client client
        this.set
            ClientId : client.get 'Id'

            
    addMaterial:(material)=>
        this.get('Materials').push material

    removeMaterial:(material)=>
        newArray = this.get('Materials')

        newArray = if newArray.length < 2 then [] else  $.grep(newArray, (v)=>v.Id != material.Id)
        this.set 
            'Materials':newArray

exports.OBNLNumberAutocompleteViewModel = OBNLNumberAutocompleteViewModel
exports.OBNLReinvestmentsList = OBNLReinvestmentsList
exports.OBNLReinvestmentModel = OBNLReinvestmentModel
exports.NewOBNLReinvestmentModel = NewOBNLReinvestmentModel