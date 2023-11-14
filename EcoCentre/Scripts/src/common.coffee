LocalizedStringLocalizer = kb.LocalizedObservable.extend({
  constructor: (value, options, view_model) ->
    kb.LocalizedObservable.prototype.constructor.apply(this, arguments)
    return kb.utils.wrappedObservable(@)
 
  read: (string_id) ->
    return kb.locale_manager.get(string_id)
})

generatePages = (current,count)->
    fp = current - 5
    fp = 1 if fp < 1
    lp = current + 5
    lp = count if lp > count
    result = []
    if fp > 1
        result.push
            number : 1 
            isActive: false
    for i in [fp..lp]
        page = 
            number:i
            isActive: i == current
        result.push page
    if lp < count
        result.push
            number : count
            isActive: false
    result

exports.LocalizedStringLocalizer = LocalizedStringLocalizer
exports.generatePages = generatePages