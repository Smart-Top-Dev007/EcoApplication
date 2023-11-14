class Model extends Backbone.Model
  url : 'dashboard'
  constructor:()->
    super
      InvoicesToday : ''
      InvoicesThisMonth : ''
      InvoicesThisYear : ''
      ClientsToday : ''
      ClientsThisMonth : ''
      ClientsThisYear: ''
      OBNLVisitsToday: ''
      OBNLVisitsThisMonth: ''
      OBNLVisitsThisYear: ''
      WeightToday: ''
      WeightThisMonth: ''
      WeightThisYear: ''
      MaxExceeded : ''
      MonthLog : undefined
      EcoCentersSummary:[]
      EcoCentersTotal :
        Clients: 0
        Visits: 0
        OBNLVisits: 0
        OBNLWeight: 0
        
      From : Date.today().moveToFirstDayOfMonth()
      To : Date.today().moveToLastDayOfMonth()
  fetch:()=>
    args =
      data :
        From : this.attributes.From.toString('yyyy-MM-dd')
        To : this.attributes.To.toString('yyyy-MM-dd')
    super args
  parse:(resp)=> 
    resp.EcoCentersTotal = 
      Visits : resp.EcoCentersSummary[0].Visits
      Clients : resp.EcoCentersSummary[0].Clients
      OBNLVisits : resp.EcoCentersSummary[0].OBNLVisits
      OBNLWeight : resp.EcoCentersSummary[0].OBNLWeight
    resp.EcoCentersSummary.splice(0, 1)
    super resp


class ViewModel extends kb.ViewModel
  constructor:()->
    super new Model()

  load:()=>
    this.model().fetch()

exports.createViewModel = ()=>
  new ViewModel()
