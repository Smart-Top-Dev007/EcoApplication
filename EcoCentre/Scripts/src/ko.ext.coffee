class LocaleManager
	constructor: (locale_identifier, @translations_by_locale) ->
		@current_locale = ko.observable(locale_identifier)
 
	get: (string_id) ->
		return string_id unless @translations_by_locale[@current_locale()]
		return string_id unless @translations_by_locale[@current_locale()].hasOwnProperty(string_id)
		return @translations_by_locale[@current_locale()][string_id]
 
	getLocale: -> return @current_locale()
	setLocale: (locale_identifier) ->
		@current_locale(locale_identifier)
		@trigger('change', @)

_.extend(LocaleManager.prototype, Backbone.Events)
delay = (ms, func) -> setTimeout func, ms
setup = ()=>
	$.get '/default/localizations',{},(data)=>
		kb.locale_manager = new LocaleManager('frCA',data)

	ko.bindingHandlers.dialog =
		init:(element,valueAccessor)=>
			dialog = ko.utils.unwrapObservable(valueAccessor())
			$(element).dialog(dialog.settings)
			dialog.isVisible.subscribe (iv)=>
				action = if iv then 'open' else 'close'
				$(element).dialog(action)
	ko.bindingHandlers.modal =
		init:(element,valueAccessor)=>
			dialog = ko.utils.unwrapObservable(valueAccessor())
			$(element).modal()
			$(element).on 'shown', ()=> 
				dialog.isVisible true
				dialog.onShown()
			$(element).on 'hidden', ()=>
				dialog.isVisible false
			initialAction = if dialog.isVisible() then 'show' else 'hide'
			$(element).modal('hide')
			$(element).delay(200).queue((t)=>$(element).modal(initialAction))

			dialog.isVisible.subscribe (iv)=>
				action = if iv then 'show' else 'hide'
				$(element).modal(action)

	ko.bindingHandlers.onKeyEnter =
		init: (element, valueAccessor, allBindingsAccessor, viewModel)=>
			callback = ko.utils.unwrapObservable(valueAccessor())
			$(element).keypress (event)=>
				keyCode = event.which
				if !event.which
					keyCode = event.keyCode
				if keyCode == 13
					callback.call(viewModel)
					return false
				return true

	ko.bindingHandlers.file =
		init: (element, valueAccessor) =>
			handler = ko.utils.unwrapObservable(valueAccessor())
			fu = $(element).fileupload handler
					
	ko.bindingHandlers.datepicker =
		init: (element, valueAccessor, allBindingsAccessor)=>
			#initialize datepicker with some optional options
			input = $(element)
			input.datepicker()
			#handle the field changing
			input.change ()=>
				newDate = input.datepicker('getDate')
				res = valueAccessor()(newDate)

			#handle disposal (if KO removes by the template binding)
			ko.utils.domNodeDisposal.addDisposeCallback(element, ()=> $(element).datepicker("destroy"))

		update: (element, valueAccessor)=>
			value = ko.utils.unwrapObservable(valueAccessor())
			$(element).datepicker("setDate", value)
		
	ko.bindingHandlers.filterHyphens =
		init: (element, valueAccessor, allBindingsAccessor, viewModel)=>
			$(element).keypress (event)=>
				keyCode = event.which
				
				if !event.which
					keyCode = event.keyCode
				if keyCode == 45
				    delay 0, ()=>
			            $(element).val($(element).val().replace(/-+/g, ' '))
				return true
			$(element).on 'paste', (event)=>
			    if window.clipboardData and window.clipboardData.getData('Text')
			        pastedText = window.clipboardData.getData('Text')
			    else
			        pastedText = event.originalEvent.clipboardData.getData('text/plain')
			        
			    if /-/.test(pastedText)
			        # this is done to move the handler to the next loop iteration
			        delay 0, ()=>
			            $(element).val($(element).val().replace(/-+/g, ' ').trim())
			            
			    return true
					
	ko.bindingHandlers.chart =
		init: (element, valueAccessor, allBindingsAccessor)=>
		update: (element, valueAccessor)=>
			data = ko.utils.unwrapObservable(valueAccessor())
			return if data == null
			element.style.width='100%';
			element.style.height='100%';
			element.width  = element.offsetWidth;
			element.height = element.offsetHeight;
			ctx = element.getContext("2d")
			options = {}
			new Chart(ctx).Bar(data);

	ko.bindingHandlers.autocomplete =
		init: (element, valueAccessor, allBindingsAccessor, viewModel) =>
			value = valueAccessor()
			options = {
				select : value.select,
				change : value.change,
				source : value.source,
				focus : ()=>false,
				minLength: 0
			}
			$(element).autocomplete(options)

	moneyHandler = (element, valueAccessor, allBindings)=>
		$el = $(element)
		valueUnwrapped = ko.unwrap( valueAccessor() )
		
		if($el.is(':input'))
			method = 'val'
		else
			method = 'text'

		return $el[method]( toMoney( valueUnwrapped ) )

	ko.bindingHandlers.money =
		update: moneyHandler

	toMoney = (num) =>
		'$' + (Number(num).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,') );


exports.setup = setup