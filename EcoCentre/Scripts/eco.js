

(function (/*! Stitch !*/) {
	if (!this.eco) {
		var modules = {}, cache = {}, require = function (name, root) {
			var path = expand(root, name), indexPath = expand(path, './index'), fn;
			var module = cache[path] || cache[indexPath];
			if (module) {
				return module;
			} else if (fn = modules[path] || modules[path = indexPath]) {
				module = { id: path, exports: {} };
				cache[path] = module.exports;
				fn(module.exports, function (name) {
					return require(name, dirname(path));
				}, module);
				return cache[path] = module.exports;
			} else {
				throw 'module ' + name + ' not found';
			}
		}, expand = function (root, name) {
			var results = [], parts, part;
			if (/^\.\.?(\/|$)/.test(name)) {
				parts = [root, name].join('/').split('/');
			} else {
				parts = name.split('/');
			}
			for (var i = 0, length = parts.length; i < length; i++) {
				part = parts[i];
				if (part == '..') {
					results.pop();
				} else if (part != '.' && part != '') {
					results.push(part);
				}
			}
			return results.join('/');
		}, dirname = function (path) {
			return path.split('/').slice(0, -1).join('/');
		};
		this.eco = {
			modules: modules,
			cache: cache,
			require: function (name) {
				return require(name, '');
			},
			define: function (bundle) {
				for (var key in bundle)
					modules[key] = bundle[key];
			}
		};
	}
	return this.eco.define;
}).call(this)({
	"client": function (exports, require, module) {
		(function () {
			var CategoriesModel, ClientFirstNameAutocompleteViewModel, ClientFirstNameAutocompleteViewModel1, ClientCivicCardAutocompleteViewModel, ClientCivicCardAutocompleteViewModel1, ClientCivicNumberAutocompleteViewModel, ClientCivicNumberAutocompleteViewModel1, ClientLastNameAutocompleteViewModel, ClientLastNameAutocompleteViewModel1, ClientList, ClientModel, ClientPostalCodeAutocompleteViewModel, ClientPostalCodeAutocompleteViewModel1, ClientStreetNameAutocompleteViewModel, ClientStreetNameAutocompleteViewModel1, common,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

			common = require('common');

			ClientLastNameAutocompleteViewModel = (function () {

				function ClientLastNameAutocompleteViewModel(value) {
					this.source = __bind(this.source, this);
					this.change = __bind(this.change, this);
					this.select = __bind(this.select, this); this.value = value;
				}

				ClientLastNameAutocompleteViewModel.prototype.select = function (e, i) {
					this.value(i.item.label);
					return false;
				};

				ClientLastNameAutocompleteViewModel.prototype.change = function (e) {
					return false;
				};

				ClientLastNameAutocompleteViewModel.prototype.source = function (request, response) {
					var data, url,
						_this = this;
					url = '/client/suggest';
					data = {
						term: request.term
					};
					return $.get(url, data, function (cat)
					{
						var arr = new Set(cat);
						cat = [...arr];
						if (!cat.length)
						{
							cat=["No result"]
						}
						return response(cat);
					});
				};

				return ClientLastNameAutocompleteViewModel;

			})();
			ClientLastNameAutocompleteViewModel1 = (function () {

				function ClientLastNameAutocompleteViewModel1(value) {
					this.source = __bind(this.source, this);
					this.change = __bind(this.change, this);
					this.select = __bind(this.select, this); this.value = value;
				}

				ClientLastNameAutocompleteViewModel1.prototype.select = function (e, i) {
					this.value(i.item.label);
					return false;
				};

				ClientLastNameAutocompleteViewModel1.prototype.change = function (e) {
					return false;
				};

				ClientLastNameAutocompleteViewModel1.prototype.source = function (request, response) {
					var data, url,
						_this = this;
					url = '/client/suggest1';
					data = {
						term: request.term
					};
					return $.get(url, data, function (cat) {
						var arr = new Set(cat);
						cat = [...arr];
						if (!cat.length) {
							cat = ["No result"]
						}
						return response(cat);
					});
				};

				return ClientLastNameAutocompleteViewModel1;

			})();

			ClientFirstNameAutocompleteViewModel = (function () {

				function ClientFirstNameAutocompleteViewModel(value) {
					this.source = __bind(this.source, this);
					this.change = __bind(this.change, this);
					this.select = __bind(this.select, this); this.value = value;
				}

				ClientFirstNameAutocompleteViewModel.prototype.select = function (e, i) {
					this.value(i.item.label);
					return false;
				};

				ClientFirstNameAutocompleteViewModel.prototype.change = function (e) {
					return false;
				};

				ClientFirstNameAutocompleteViewModel.prototype.source = function (request, response) {
					var data, url,
						_this = this;
					url = '/client/suggest';
					data = {
						term: request.term,
						isFirstName: true
					};
					return $.get(url, data, function (cat)
					{
						var arr = new Set(cat);
						cat = [...arr];
						if (!cat.length)
						{
							cat = ["No result"]
						}
						return response(cat);
					});
				};

				return ClientFirstNameAutocompleteViewModel;

			})();
			ClientFirstNameAutocompleteViewModel1 = (function () {

				function ClientFirstNameAutocompleteViewModel1(value) {
					this.source = __bind(this.source, this);
					this.change = __bind(this.change, this);
					this.select = __bind(this.select, this); this.value = value;
				}

				ClientFirstNameAutocompleteViewModel1.prototype.select = function (e, i) {
					this.value(i.item.label);
					return false;
				};

				ClientFirstNameAutocompleteViewModel1.prototype.change = function (e) {
					return false;
				};

				ClientFirstNameAutocompleteViewModel1.prototype.source = function (request, response) {
					var data, url,
						_this = this;
					url = '/client/suggest1';
					data = {
						term: request.term,
						isFirstName: true
					};
					return $.get(url, data, function (cat) {
						var arr = new Set(cat);
						cat = [...arr];
						if (!cat.length) {
							cat = ["No result"]
						}
						return response(cat);
					});
				};

				return ClientFirstNameAutocompleteViewModel1;

			})();

			ClientStreetNameAutocompleteViewModel = (function () {

				function ClientStreetNameAutocompleteViewModel(number, streetName, postalCode, cityId, hubId) {
					this.source = __bind(this.source, this);
					this.change = __bind(this.change, this);
					this.select = __bind(this.select, this); this.number = number;
					this.streetName = streetName;
					this.number = number;
                    this.postalCode = postalCode;
                    this.cityId = cityId;
					this.hubId = hubId;
				}

				ClientStreetNameAutocompleteViewModel.prototype.select = function (e, i) {
					this.streetName(i.item.label);
					return false;
				};

				ClientStreetNameAutocompleteViewModel.prototype.change = function (e) {
					return false;
				};

				ClientStreetNameAutocompleteViewModel.prototype.source = function (request, response) {
					var data, hubId, url,
						_this = this;
					url = '/client/suggeststreet';
					hubId = this.hubId;
					if (typeof hubId === "function") hubId = hubId();
					if (!hubId) hubId = "";
					hubId = "";
					data = {
						streetName: request.term || '',
						cityId: this.cityId || '',
						hubId: hubId || ''
					};
					return $.get(url, data, function (cat)
					{
						var arr = new Set(cat);
						cat = [...arr];
						if (!cat.length)
						{
							cat = ["No result"]
						}
						return response(cat);
					});
				};

				return ClientStreetNameAutocompleteViewModel;

			})();
			ClientStreetNameAutocompleteViewModel1 = (function () {

				function ClientStreetNameAutocompleteViewModel1(number, streetName, postalCode, cityId, hubId) {
					this.source = __bind(this.source, this);
					this.change = __bind(this.change, this);
					this.select = __bind(this.select, this); this.number = number;
					this.streetName = streetName;
					this.number = number;
                    this.postalCode = postalCode;
                    this.cityId = cityId;
					this.hubId = hubId;
				}

				ClientStreetNameAutocompleteViewModel1.prototype.select = function (e, i) {
					this.streetName(i.item.label);
					return false;
				};

				ClientStreetNameAutocompleteViewModel1.prototype.change = function (e) {
					return false;
				};

				ClientStreetNameAutocompleteViewModel1.prototype.source = function (request, response) {
					var data, hubId, url,
						_this = this;
					url = '/client/suggeststreet1';
					hubId = this.hubId;
					if (typeof hubId === "function") hubId = hubId();
					if (!hubId) hubId = "";
					hubId = "";
					data = {
						streetName: request.term || '',
						cityId: this.cityId || '',
						hubId: hubId || ''
					};
					return $.get(url, data, function (cat)
					{
						var arr = new Set(cat);
						cat = [...arr];
						if (!cat.length)
						{
							cat = ["No result"]
						}
						return response(cat);
					});
				};

				return ClientStreetNameAutocompleteViewModel1;

			})();

			ClientCivicNumberAutocompleteViewModel = (function () {

				function ClientCivicNumberAutocompleteViewModel(number, streetName, postalCode) {
					this.source = __bind(this.source, this);
					this.change = __bind(this.change, this);
					this.select = __bind(this.select, this); this.number = number;
					console.log('number>>>>', number)
					this.streetName = streetName;
					this.number = number;
					this.postalCode = postalCode;
				}

				ClientCivicNumberAutocompleteViewModel.prototype.select = function (e, i) {
					this.number(i.item.label);
					return false;
				};

				ClientCivicNumberAutocompleteViewModel.prototype.change = function (e) {
					return false;
				};

				ClientCivicNumberAutocompleteViewModel.prototype.source = function (request, response) {
					var data, url,
						_this = this;
					url = '/client/suggestcivicnumber';
					data = {
						number: request.term || '',
						streetName: this.streetName || '',
						postalCode: this.postalCode || ''
					};
					return $.get(url, data, function (cat)
					{
						var arr = new Set(cat);
						cat = [...arr];
						if (!cat.length)
						{
							cat = ["No result"]
						}
						return response(cat);
					});
				};

				return ClientCivicNumberAutocompleteViewModel;

			})();
			ClientCivicNumberAutocompleteViewModel1 = (function () {

				function ClientCivicNumberAutocompleteViewModel1(number, streetName, postalCode) {
					this.source = __bind(this.source, this);
					this.change = __bind(this.change, this);
					this.select = __bind(this.select, this); this.number = number;
					console.log('number>>>>', number)
					this.streetName = streetName;
					this.number = number;
					this.postalCode = postalCode;
				}

				ClientCivicNumberAutocompleteViewModel1.prototype.select = function (e, i) {
					this.number(i.item.label);
					return false;
				};

				ClientCivicNumberAutocompleteViewModel1.prototype.change = function (e) {
					return false;
				};

				ClientCivicNumberAutocompleteViewModel1.prototype.source = function (request, response) {
					var data, url,
						_this = this;
					url = '/client/suggestcivicnumber1';
					data = {
						number: request.term || '',
						streetName: this.streetName || '',
						postalCode: this.postalCode || ''
					};
					return $.get(url, data, function (cat) {
						var arr = new Set(cat);
						cat = [...arr];
						if (!cat.length) {
							cat = ["No result"]
						}
						return response(cat);
					});
				};

				return ClientCivicNumberAutocompleteViewModel1;

			})();

			ClientCivicCardAutocompleteViewModel = (function () {

				function ClientCivicCardAutocompleteViewModel(card) {
					this.source = __bind(this.source, this);
					this.change = __bind(this.change, this);
					this.select = __bind(this.select, this);
					console.log('card >>>>>', card)
					this.card = card;
					this.card = card;
				}

				ClientCivicCardAutocompleteViewModel.prototype.select = function (e, i) {
					console.log(i)
					console.log(this.card)
					this.card(i.item.label);
					return false;
				};

				ClientCivicCardAutocompleteViewModel.prototype.change = function (e) {
					return false;
				};

				ClientCivicCardAutocompleteViewModel.prototype.source = function (request, response) {
					var data, url,
						_this = this;
					url = '/client/suggestciviccard';
					data = {
						card: request.term || '',
					};
					return $.get(url, data, function (cat)
					{
						var arr = new Set(cat);
						cat = [...arr];
						if (!cat.length)
						{
							cat = ["No result"]
						}
						return response(cat);
					});
				};

				return ClientCivicCardAutocompleteViewModel;

			})();
			ClientCivicCardAutocompleteViewModel1 = (function () {

				function ClientCivicCardAutocompleteViewModel1(card) {
					this.source = __bind(this.source, this);
					this.change = __bind(this.change, this);
					this.select = __bind(this.select, this);
					console.log('card >>>>>', card)
					this.card = card;
					this.card = card;
				}

				ClientCivicCardAutocompleteViewModel1.prototype.select = function (e, i) {
					console.log(i)
					console.log(this.card)
					this.card(i.item.label);
					return false;
				};

				ClientCivicCardAutocompleteViewModel1.prototype.change = function (e) {
					return false;
				};

				ClientCivicCardAutocompleteViewModel1.prototype.source = function (request, response) {
					var data, url,
						_this = this;
					url = '/client/suggestciviccard1';
					data = {
						card: request.term || '',
					};
					return $.get(url, data, function (cat) {
						var arr = new Set(cat);
						cat = [...arr];
						if (!cat.length) {
							cat = ["No result"]
						}
						return response(cat);
					});
				};

				return ClientCivicCardAutocompleteViewModel1;

			})();


			ClientPostalCodeAutocompleteViewModel = (function () {

				function ClientPostalCodeAutocompleteViewModel(postalCode, number, streetName, ) {
					this.source = __bind(this.source, this);
					this.change = __bind(this.change, this);
					this.select = __bind(this.select, this); this.number = number;
					this.streetName = streetName;
					this.number = number;
					this.postalCode = postalCode;
				}

				ClientPostalCodeAutocompleteViewModel.prototype.select = function (e, i) {
					this.postalCode(i.item.label);
					return false;
				};

				ClientPostalCodeAutocompleteViewModel.prototype.change = function (e) {
					return false;
				};

				ClientPostalCodeAutocompleteViewModel.prototype.source = function (request, response) {
					var data, url,
						_this = this;
					url = '/client/suggestpostalcode';
					data = {
						postalCode: request.term || '',
						number: this.number || '',
						streetName: this.streetName || '',
						
					};
					return $.get(url, data, function (cat)
					{
						var arr = new Set(cat);
						cat = [...arr];
						if (!cat.length)
						{
							cat = ["No result"]
						}
						return response(cat);
					});
				};

				return ClientPostalCodeAutocompleteViewModel;

			})();
			ClientPostalCodeAutocompleteViewModel1 = (function () {

				function ClientPostalCodeAutocompleteViewModel1(postalCode, number, streetName,) {
					this.source = __bind(this.source, this);
					this.change = __bind(this.change, this);
					this.select = __bind(this.select, this); this.number = number;
					this.streetName = streetName;
					this.number = number;
					this.postalCode = postalCode;
				}

				ClientPostalCodeAutocompleteViewModel1.prototype.select = function (e, i) {
					this.postalCode(i.item.label);
					return false;
				};

				ClientPostalCodeAutocompleteViewModel1.prototype.change = function (e) {
					return false;
				};

				ClientPostalCodeAutocompleteViewModel1.prototype.source = function (request, response) {
					var data, url,
						_this = this;
					url = '/client/suggestpostalcode1';
					data = {
						postalCode: request.term || '',
						number: this.number || '',
						streetName: this.streetName || '',

					};
					return $.get(url, data, function (cat) {
						var arr = new Set(cat);
						cat = [...arr];
						if (!cat.length) {
							cat = ["No result"]
						}
						return response(cat);
					});
				};

				return ClientPostalCodeAutocompleteViewModel1;

			})();

			CategoriesModel = (function (_super) {

				__extends(CategoriesModel, _super);

				CategoriesModel.prototype.url = 'ClientCategories';

				function CategoriesModel() {
					this.showOBNLNumber = __bind(this.showOBNLNumber, this);
					this.showOrganizationName = __bind(this.showOrganizationName, this);
					this.showFirstNameLastName = __bind(this.showFirstNameLastName, this);
					this.parse = __bind(this.parse, this); CategoriesModel.__super__.constructor.call(this);
					this.list = new Backbone.Collection();
				}

				CategoriesModel.prototype.parse = function (resp) {
					var item, _i, _len, _results;
					_results = [];
					for (_i = 0, _len = resp.length; _i < _len; _i++) {
						item = resp[_i];
						_results.push(this.list.push(item));
					}
					return _results;
				};

				CategoriesModel.prototype.showFirstNameLastName = function (category) {
					var item, _i, _len, _ref;
					_ref = this.items;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						if (item.CategoryName() === category) return item.ShowFirstNameLastName();
					}
					return true;
				};

				CategoriesModel.prototype.showOrganizationName = function (category) {
					var item, _i, _len, _ref;
					_ref = this.items;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						if (item.CategoryName() === category) return item.ShowOrganizationName();
					}
					return true;
				};

				CategoriesModel.prototype.showOBNLNumber = function (category) {
					var item, _i, _len, _ref;
					_ref = this.items;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						if (item.CategoryName() === category) return item.ShowOBNLNumber();
					}
					return true;
				};

				return CategoriesModel;

			})(Backbone.Model);

			ClientModel = (function (_super) {

				__extends(ClientModel, _super);

				ClientModel.prototype.urlRoot = 'client/index';

				ClientModel.prototype.idAttribute = 'Id';

				function ClientModel(data) {
					if (data == null) data = null;
					this.parseDate = __bind(this.parseDate, this);
					this.parse = __bind(this.parse, this);
					this.Invoices = new Backbone.Collection();
					this.ExcludedInvoices = new Backbone.Collection();
					this.IncludedInvoices = new Backbone.Collection();
					this.OBNLReinvestments = new Backbone.Collection();
					this.ExcludedOBNLReinvestments = new Backbone.Collection();
					this.IncludedOBNLReinvestments = new Backbone.Collection();
					ClientModel.__super__.constructor.call(this, {
						AllowCredit: false,
						CreditAcountNumber: '',
						AllowAddressCreation: false,
						Id: null,
						RefId: null,
						FirstName: '',
						LastName: '',
						OBNLNumber: '',
						OBNLNumbers: [],
						Category: 'Resident',
						CategoryCustom: '',
						LastChange:'',
						CitizenCard: '',
						Email: '',
						PhoneNumber: '',
						MobilePhoneNumber: '',
						Hub: {
							Id: null,
							Name: ''
						},
						Comments: '',
						PersonalVisitsLimit: 0,
						Status: '',
						Verified: '',
						Address: {
							Street: '',
							City: '',
							CityId: '',
							CivicNumber: '',
							AptNumber: '',
							PostalCode: '',
							ExternalId: '',
							NewCityName: ''
						},
						UpdateOnlyStatus: false,
						IsRegisteredInCurrentHub: true
					});
					if (data != null) this.parse(data);
				}

				ClientModel.prototype.parse = function (resp) {
					var inv, reinv, _i, _j, _len, _len2, _ref, _ref2;
					if (resp.Invoices) {
						_ref = resp.Invoices;
						for (_i = 0, _len = _ref.length; _i < _len; _i++) {
							inv = _ref[_i];
							inv.CenterUrl = inv.Center.Url;
							this.Invoices.push(inv);
							if (inv.IsExcluded) {
								this.ExcludedInvoices.push(inv);
							} else {
								this.IncludedInvoices.push(inv);
							}
						}
					}
					if (resp.OBNLReinvestments) {
						_ref2 = resp.OBNLReinvestments;
						for (_j = 0, _len2 = _ref2.length; _j < _len2; _j++) {
							reinv = _ref2[_j];
							reinv.CenterUrl = reinv.Center.Url;
							this.OBNLReinvestments.push(reinv);
							if (reinv.IsExcluded) {
								this.ExcludedOBNLReinvestments.push(reinv);
							} else {
								this.IncludedOBNLReinvestments.push(reinv);
							}
						}
                    }
					this.set({
						AllowCredit: resp.AllowCredit,
						CreditAcountNumber: resp.CreditAcountNumber,
						AllowAddressCreation: resp.AllowAddressCreation,
						Id: resp.Id,
						RefId: resp.RefId,
						FirstName: resp.FirstName,
						LastName: resp.LastName,
						OBNLNumber: resp.OBNLNumber,
						OBNLNumbers: resp.OBNLNumbers,
						LastOBNLVisit: this.parseDate(resp.LastOBNLVisit),
						Category: resp.Category,
						MobilePhoneNumber: resp.MobilePhoneNumber,
						Comments: resp.Comments,
						CategoryCustom: resp.CategoryCustom,
						LastChange: resp.LastChange ? new Date(+resp.LastChange.match(/\d+/i)[0]).toString():'',
						Status: resp.Status,
						Verified: resp.Verified,
						PersonalVisitsLimit: resp.PersonalVisitsLimit,
						IsRegisteredInCurrentHub: true//resp.IsRegisteredInCurrentHub
                    });
                    this.set("CitizenCard", resp.CitizenCard);
                    this.set("Email", resp.Email);
					this.set("PhoneNumber", resp.PhoneNumber);
					this.set("LastChange", resp.LastChange ? resp.LastChange.split(" ")[0].split("-").join("/") : 'NV');
                    this.set({
						'Address.Street': resp.Address.Street,
						'Address.City': resp.Address.City,
						'Address.CityId': resp.Address.CityId,
						'Address.CivicNumber': resp.Address.CivicNumber,
						'Address.AptNumber': resp.Address.AptNumber,
						'Address.PostalCode': resp.Address.PostalCode,
						'Address.AllowAddressCreation': resp.Address.AllowAddressCreation,
						'Address.ExternalId': resp.Address.ExternalId
                    });                  
					if (resp.Hub) {
						this.set({
							'Hub.Id': resp.Hub.Id,
							'Hub.Name': resp.Hub.Name,
						});
					}
					this.change();
					return this.attributes;
				};

				ClientModel.prototype.parseDate = function (item) {
					var ca, res;
					if (!item) return;
					res = item.match(/\d+/g)[0];
					ca = new Date(res * 1);
					return ca.toString('yyyy-MM-dd');
				};

				return ClientModel;

			})(Backbone.DeepModel);

			ClientModel1 = (function (_super) {

				__extends(ClientModel1, _super);

				ClientModel1.prototype.urlRoot = 'client/canadianindex';

				ClientModel1.prototype.idAttribute = 'Id';

				function ClientModel1(data) {
					if (data == null) data = null;
					this.parseDate = __bind(this.parseDate, this);
					this.parse = __bind(this.parse, this);
					this.Invoices = new Backbone.Collection();
					this.ExcludedInvoices = new Backbone.Collection();
					this.IncludedInvoices = new Backbone.Collection();
					this.OBNLReinvestments = new Backbone.Collection();
					this.ExcludedOBNLReinvestments = new Backbone.Collection();
					this.IncludedOBNLReinvestments = new Backbone.Collection();
					ClientModel1.__super__.constructor.call(this, {
						AllowCredit: false,
						CreditAcountNumber: '',
						AllowAddressCreation: false,
						Id: null,
						RefId: null,
						FirstName: '',
						LastName: '',
						OBNLNumber: '',
						OBNLNumbers: [],
						Category: 'Resident',
						CategoryCustom: '',
						LastChange: '',
						CitizenCard: '',
						Email: '',
						PhoneNumber: '',
						MobilePhoneNumber: '',
						Hub: {
							Id: null,
							Name: ''
						},
						Comments: '',
						PersonalVisitsLimit: 0,
						Status: '',
						Verified: '',
						Address: {
							Street: '',
							City: '',
							CityId: '',
							CivicNumber: '',
							AptNumber: '',
							PostalCode: '',
							ExternalId: '',
							NewCityName: ''
						},
						UpdateOnlyStatus: false,
						IsRegisteredInCurrentHub: true
					});
					if (data != null) this.parse(data);
				}

				ClientModel1.prototype.parse = function (resp) {
					var inv, reinv, _i, _j, _len, _len2, _ref, _ref2;
					if (resp.Invoices) {
						_ref = resp.Invoices;
						for (_i = 0, _len = _ref.length; _i < _len; _i++) {
							inv = _ref[_i];
							inv.CenterUrl = inv.Center.Url;
							this.Invoices.push(inv);
							if (inv.IsExcluded) {
								this.ExcludedInvoices.push(inv);
							} else {
								this.IncludedInvoices.push(inv);
							}
						}
					}
					if (resp.OBNLReinvestments) {
						_ref2 = resp.OBNLReinvestments;
						for (_j = 0, _len2 = _ref2.length; _j < _len2; _j++) {
							reinv = _ref2[_j];
							reinv.CenterUrl = reinv.Center.Url;
							this.OBNLReinvestments.push(reinv);
							if (reinv.IsExcluded) {
								this.ExcludedOBNLReinvestments.push(reinv);
							} else {
								this.IncludedOBNLReinvestments.push(reinv);
							}
						}
					}
					this.set({
						AllowCredit: resp.AllowCredit,
						CreditAcountNumber: resp.CreditAcountNumber,
						AllowAddressCreation: resp.AllowAddressCreation,
						Id: resp.Id,
						RefId: resp.RefId,
						FirstName: resp.FirstName,
						LastName: resp.LastName,
						OBNLNumber: resp.OBNLNumber,
						OBNLNumbers: resp.OBNLNumbers,
						LastOBNLVisit: this.parseDate(resp.LastOBNLVisit),
						Category: resp.Category,
						MobilePhoneNumber: resp.MobilePhoneNumber,
						Comments: resp.Comments,
						CategoryCustom: resp.CategoryCustom,
						LastChange: resp.LastChange ? new Date(+resp.LastChange.match(/\d+/i)[0]).toString() : '',
						Status: resp.Status,
						Verified: resp.Verified,
						PersonalVisitsLimit: resp.PersonalVisitsLimit,
						IsRegisteredInCurrentHub: true//resp.IsRegisteredInCurrentHub
					});
					this.set("CitizenCard", resp.CitizenCard);
					this.set("Email", resp.Email);
					this.set("PhoneNumber", resp.PhoneNumber);
					this.set("LastChange", resp.LastChange ? resp.LastChange.split(" ")[0].split("-").join("/") : 'NV');
					this.set({
						'Address.Street': resp.Address.Street,
						'Address.City': resp.Address.City,
						'Address.CityId': resp.Address.CityId,
						'Address.CivicNumber': resp.Address.CivicNumber,
						'Address.AptNumber': resp.Address.AptNumber,
						'Address.PostalCode': resp.Address.PostalCode,
						'Address.AllowAddressCreation': resp.Address.AllowAddressCreation,
						'Address.ExternalId': resp.Address.ExternalId
					});
					if (resp.Hub) {
						this.set({
							'Hub.Id': resp.Hub.Id,
							'Hub.Name': resp.Hub.Name,
						});
					}
					this.change();
					return this.attributes;
				};

				ClientModel1.prototype.parseDate = function (item) {
					var ca, res;
					if (!item) return;
					res = item.match(/\d+/g)[0];
					ca = new Date(res * 1);
					return ca.toString('yyyy-MM-dd');
				};

				return ClientModel1;

			})(Backbone.DeepModel);

			ClientList = (function (_super) {

				__extends(ClientList, _super);

				ClientList.prototype.clients = new Backbone.Collection();

				ClientList.prototype.pageButtons = new Backbone.Collection();

				ClientList.prototype.url = 'client';

				function ClientList() {
					this.changePage = __bind(this.changePage, this);
					this.search = __bind(this.search, this);
					this.parse = __bind(this.parse, this); ClientList.__super__.constructor.call(this, {
						filterFirstName: '',
						filterLastName: '',
						filterAddress: '',
						filterCivicNumber: '',
						filterPostalCode:'',
						filterLastVisitFrom: '',
						filterLastVisitTo: '',
						filterFirstLetter: '',
						filterHubId: '',
						filterOBNLNumber: '',
						filterCategory: 'All',
						filterActive: true,
						filterInactive: false,
						filterVerified: true,
						filterNoVerified: false,
						filterCitizenCard: '',
						filterLastChange:'', 
						noCommercial: false,
						sortBy: 'LastName',
						sortDir: 'Asc',
						searchType: 'Address',
						filterType: 'active',
						term: '',
						page: 1,
						pageCount: 1,
						pageSize: null
					});
				}

				ClientList.prototype.parse = function (resp) {
					var item, page, pages, _i, _j, _len, _len2, _ref, _results;
					this.clients.reset();
					_ref = resp.Clients;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						this.clients.push(new ClientModel(item));
					}
					this.pageButtons.reset();
					pages = common.generatePages(resp.Page, resp.PageCount);
					this.set('page', resp.Page);
					this.set('pageCount', resp.PageCount);
					_results = [];
					for (_j = 0, _len2 = pages.length; _j < _len2; _j++) {
						page = pages[_j];
						_results.push(this.pageButtons.push(page));
					}
					return _results;
				};

				ClientList.prototype.search = function () {
					return this.changePage(1);
				};

				ClientList.prototype.changePage = function (page) {
					var $loadingFade, data, fetchAjax,
						_this = this;
					data = {
						page: page,
						pageSize: this.attributes.pageSize,
						firstName: this.attributes.filterFirstName,
						lastName: this.attributes.filterLastName,
						address: this.attributes.filterAddress,
						citizenCard: this.attributes.filterCitizenCard,
						lastChange: this.attributes.filterLastChange,
						civicNumber: this.attributes.filterCivicNumber,
						postalCode: this.attributes.filterPostalCode,
						OBNLNumber: this.attributes.filterOBNLNumber,
						firstLetter: this.attributes.filterFirstLetter,
						hubId: this.attributes.filterHubId,
						categoryFilter: this.attributes.filterCategory,
						active: this.attributes.filterActive,
						inactive: this.attributes.filterInactive,
						verified: this.attributes.filterVerified,
						noverified: this.attributes.filterNoVerified,
						noCommercial: this.attributes.noCommercial,
						sortDir: this.attributes.sortDir,
						sortBy: this.attributes.sortBy,
						lastVisitFrom: this.attributes.filterLastVisitFrom.toString('yyyy-MM-dd'),
						lastVisitTo: this.attributes.filterLastVisitTo.toString('yyyy-MM-dd'),
						term: this.attributes.term,
						searchType: this.attributes.searchType
					};
					$loadingFade = $("#global-loading-fade");
					$loadingFade.modal('show');
					fetchAjax = this.fetch({
						data: data
					});
					return fetchAjax.complete(function () {
						var setTimeoutCallback;
						var counter = 0;
						setTimeoutCallback = function () {
							var result = $('#client-list').find('tr');
							if (result.length === 2) {
								result[1].click();
								clearInterval(searchInterval);
							} else {
								++counter;
							}
							if (counter > 30) {
								counter = 0;
								clearInterval(searchInterval);
							}
							return $loadingFade.modal('hide');
						};
						setTimeout(setTimeoutCallback, 500);
						return fetchAjax;
					});
				};

				return ClientList;

			})(Backbone.Model);

			exports.ClientList = ClientList;

			exports.Model = ClientModel;

			exports.CandianModel = ClientModel1;

			exports.CategoriesModel = CategoriesModel;

			exports.ClientLastNameAutocompleteViewModel = ClientLastNameAutocompleteViewModel;
			exports.ClientLastNameAutocompleteViewModel1 = ClientLastNameAutocompleteViewModel1;

			exports.ClientFirstNameAutocompleteViewModel = ClientFirstNameAutocompleteViewModel;
			exports.ClientFirstNameAutocompleteViewModel1 = ClientFirstNameAutocompleteViewModel1;

			exports.ClientStreetNameAutocompleteViewModel = ClientStreetNameAutocompleteViewModel;
			exports.ClientStreetNameAutocompleteViewModel1 = ClientStreetNameAutocompleteViewModel1;

			exports.ClientCivicNumberAutocompleteViewModel = ClientCivicNumberAutocompleteViewModel;
			exports.ClientCivicNumberAutocompleteViewModel1 = ClientCivicNumberAutocompleteViewModel1;

			exports.ClientCivicCardAutocompleteViewModel = ClientCivicCardAutocompleteViewModel;
			exports.ClientCivicCardAutocompleteViewModel1 = ClientCivicCardAutocompleteViewModel1;

			exports.ClientPostalCodeAutocompleteViewModel = ClientPostalCodeAutocompleteViewModel;
			exports.ClientPostalCodeAutocompleteViewModel1 = ClientPostalCodeAutocompleteViewModel1;

		}).call(this);
	}, "client.form": function (exports, require, module) {
		(function () {
			var AddressViewModel, HubViewModel, ViewModel, client,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			client = require('client');

			HubViewModel = (function () {

				function HubViewModel(model) {
					this.updateId = __bind(this.updateId, this);
					this.updateName = __bind(this.updateName, this); this.Name = kb.observable(model, 'Hub.Name');
					this.Id = kb.observable(model, 'Hub.Id');
				}

				HubViewModel.prototype.updateName = function (m, v) {
					return this.Name(v);
				};

				HubViewModel.prototype.updateId = function (m, v) {
					return this.Id(v);
				};

				return HubViewModel;

			})();

			AddressViewModel = (function () {

				function AddressViewModel(model) {
					this.updateCityId = __bind(this.updateCityId, this);
					this.updateCivicNumber = __bind(this.updateCivicNumber, this);
					this.updateAptNumber = __bind(this.updateAptNumber, this);
					this.updateCity = __bind(this.updateCity, this);
					this.updateStreet = __bind(this.updateStreet, this);
					this.updatePostalCode = __bind(this.updatePostalCode, this); this.City = kb.observable(model, 'Address.City');
					this.Street = kb.observable(model, 'Address.Street');
					this.PostalCode = kb.observable(model, 'Address.PostalCode');
					this.AptNumber = kb.observable(model, 'Address.AptNumber');
					this.CivicNumber = kb.observable(model, 'Address.CivicNumber');
					this.AptNumber = kb.observable(model, 'Address.AptNumber');
					this.CityId = kb.observable(model, 'Address.CityId');
					this.NewCityName = kb.observable(model, 'Address.NewCityName');
					this.streetNameAutocomplete = new client.ClientStreetNameAutocompleteViewModel(this.CivicNumber, this.Street, this.PostalCode, this.CityId);
					this.civicNumberAutocomplete = new client.ClientCivicNumberAutocompleteViewModel(this.CivicNumber, this.Street, this.PostalCode);
					this.postalCodeAutocomplete = new client.ClientPostalCodeAutocompleteViewModel(this.CivicNumber, this.Street, this.PostalCode);
					model.on('change:Address.PostalCode', this.updatePostalCode);
					model.on('change:Address.Street', this.updateStreet);
					model.on('change:Address.City', this.updateCity);
					model.on('change:Address.CityId', this.updateCityId);
					model.on('change:Address.CivicNumber', this.updateCivicNumber);
					model.on('change:Address.AptNumber', this.updateAptNumber);
				}

				AddressViewModel.prototype.updatePostalCode = function (m, v) {
					return this.PostalCode(v);
				};

				AddressViewModel.prototype.updateStreet = function (m, v) {
					return this.Street(v);
				};

				AddressViewModel.prototype.updateCity = function (m, v) {
					return this.City(v);
				};

				AddressViewModel.prototype.updateAptNumber = function (m, v) {
					return this.AptNumber(v);
				};

				AddressViewModel.prototype.updateCivicNumber = function (m, v) {
					return this.CivicNumber(v);
				};

                AddressViewModel.prototype.updateCityId = function (m, v) {
                    var cityId = "";
					var city = this.City();


					if ($("#selectCity")[0])
					{
						if ($("#selectCity")[0].options[$("#selectCity")[0].selectedIndex].label === "Autre")
						{
							$($("#selectCity")[0].options).each(function (i, e)
							{
								if (e.label === city)
								{
									cityId = e.value;
								}
							});
							return this.CityId(cityId);
						} else
						{
							return this.CityId(v);
						}
					}
				};

				return AddressViewModel;

			})();

			ViewModel = (function (_super) {

				__extends(ViewModel, _super);

				function ViewModel(model, municipalities, categoriesModel, hubs, currentUserModel) {
					this.onError = __bind(this.onError, this);
					this.onSaved = __bind(this.onSaved, this);
					this.save = __bind(this.save, this);
					this.onCurrentUserLoaded = __bind(this.onCurrentUserLoaded, this);
					this.loadNew = __bind(this.loadNew, this);
					this.load = __bind(this.load, this);
					this.isNew = __bind(this.isNew, this);
					this.addNewOBNL = __bind(this.addNewOBNL, this);
					this.computeShowOBNLNumber = __bind(this.computeShowOBNLNumber, this);
					this.computeShowOrganizationName = __bind(this.computeShowOrganizationName, this);
					this.computeShowFirstNameLastName = __bind(this.computeShowFirstNameLastName, this);
					this.onMunicipalitiesReloaded = __bind(this.onMunicipalitiesReloaded, this);
					this.computeShowHubs = __bind(this.computeShowHubs, this);
					this.computeShowCustomCategoryName = __bind(this.computeShowCustomCategoryName, this); ViewModel.__super__.constructor.call(this, model);
					this.municipalitiesModel = municipalities;
					this.address = ko.observable(new AddressViewModel(model));
					this.hub = ko.observable(new HubViewModel(model));
					this.errors = ko.observableArray([]);
					this.OBNLNumbersInputed = ko.observableArray([]);
					this.categoriesModel = categoriesModel;
					this.currentUserModel = currentUserModel;
					this.Municipalities = ko.observableArray([]);
					municipalities.on('reloaded', this.onMunicipalitiesReloaded);
					this.onMunicipalitiesReloaded();
					this.Categories = kb.collectionObservable(categoriesModel.list);
					this.showFirstNameLastName = ko.computed(this.computeShowFirstNameLastName);
					this.hubs = kb.collectionObservable(hubs.hubs);
					this.showHub = ko.computed(this.computeShowHubs);
					this.showOrganizationName = ko.computed(this.computeShowOrganizationName);
					this.showCustomCategoryName = ko.computed(this.computeShowCustomCategoryName);
					this.showOBNLNumber = ko.computed(this.computeShowOBNLNumber);
					this.allowAddressCreation = kb.observable(model, 'AllowAddressCreation');
					this.NewCityName = ko.observable('');
				}

				ViewModel.prototype.computeShowCustomCategoryName = function () {
					return this.Category() === "Other";
				};

				ViewModel.prototype.computeShowHubs = function () {
					return this.hubs().length > 0;
				};

				ViewModel.prototype.onMunicipalitiesReloaded = function () {
					var m, _i, _len, _ref;
					this.Municipalities.removeAll();
					_ref = this.municipalitiesModel.municipalities.models;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						m = _ref[_i];
						this.Municipalities.push(m.attributes);
					}
					return this.Municipalities.push({
						Id: "",
						Name: "Autre"
					});
				};

				ViewModel.prototype.computeShowFirstNameLastName = function () {
					var item, _i, _len, _ref;
					_ref = this.Categories();
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						if (item.CategoryName() === this.Category()) {
							return item.ShowFirstNameLastName();
						}
					}
					return true;
				};

				ViewModel.prototype.computeShowOrganizationName = function () {
					var item, _i, _len, _ref;
					_ref = this.Categories();
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						if (item.CategoryName() === this.Category()) {
							return item.ShowOrganizationName();
						}
					}
					return false;
				};

				ViewModel.prototype.computeShowOBNLNumber = function () {
					return this.Category() === 'OBNL';
				};

				ViewModel.prototype.addNewOBNL = function () {
					if (this.OBNLNumber()) {
						if (this.OBNLNumbers() === null) this.OBNLNumbers([]);
						this.OBNLNumbers().push(this.OBNLNumber());
						this.OBNLNumbersInputed.push(this.OBNLNumber());
						return this.OBNLNumber('');
					}
				};

				ViewModel.prototype.isNew = function () {
					return this.model().isNew();
				};

				ViewModel.prototype.load = function (id) {
					return this.model().fetch({
						data: {
							id: id
						}
					});
				};

				ViewModel.prototype.loadNew = function (id) {
					return this.currentUserModel.fetch({
						success: this.onCurrentUserLoaded
					});
				};

				ViewModel.prototype.onCurrentUserLoaded = function () {
					this.model().set('Hub.Id', this.currentUserModel.get("HubId"));
					return this.model().set('Address.CityId', this.currentUserModel.get("DefaultCityId"));
				};

				ViewModel.prototype.save = function () {
					return this.model().save(null, {
						success: this.onSaved,
						error: this.onError
					});
				};

				ViewModel.prototype.save1 = function () {
					return this.model().save(null, {
						success: this.onSaved,
						error: this.onError
					});
				};

				ViewModel.prototype.onSaved = function (data) {
					eco.app.notifications.addNotification('msg-client-saved-successfully');
					return eco.app.router.navigate('client/index', {
						trigger: true
					});
				};

				ViewModel.prototype.onError = function (m, errors, data) {
					var confirmationText, item, _i, _len, _results;
					errors = $.parseJSON(errors.responseText);
					if (errors.length === 1 && errors[0].PropertyName === 'AllowAddressCreation') {
						confirmationText = kb.locale_manager.get("This address already exists in the database. Are you sure you want to create a customer at this address?");
						if (confirm(confirmationText)) {
							this.model().set('AllowAddressCreation', true);
							return this.save();
						}
					} else {
						this.errors.removeAll();
						_results = [];
						for (_i = 0, _len = errors.length; _i < _len; _i++) {
							item = errors[_i];
							_results.push(this.errors.push(item));
						}
						return _results;
					}
				};

				return ViewModel;

			})(kb.ViewModel);

			exports.createViewModel = function () {
				var categories, clientModule, currentUserModel, hubs, model, municipalities;
				clientModule = require('client');
				model = new clientModule.Model();
				municipalities = new (require('municipality')).ListModel();
				municipalities.filter = function (item) {
					return item.Enabled;
				};
				municipalities.fetch({
					async: false
				});
				hubs = new (require('hub')).ListModel();
				hubs.fetch();
				categories = new clientModule.CategoriesModel();
				categories.fetch();
				currentUserModel = new (require('user')).CurrentUserModel();
				return new ViewModel(model, municipalities, categories, hubs, currentUserModel);
			};

		}).call(this);
	}, "client.canadian.form": function (exports, require, module) {
		(function () {
			var AddressViewModel, HubViewModel, ViewModel, client,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			client = require('client');

			HubViewModel = (function () {

				function HubViewModel(model) {
					this.updateId = __bind(this.updateId, this);
					this.updateName = __bind(this.updateName, this); this.Name = kb.observable(model, 'Hub.Name');
					this.Id = kb.observable(model, 'Hub.Id');
				}

				HubViewModel.prototype.updateName = function (m, v) {
					return this.Name(v);
				};

				HubViewModel.prototype.updateId = function (m, v) {
					return this.Id(v);
				};

				return HubViewModel;

			})();

			AddressViewModel = (function () {

				function AddressViewModel(model) {
					this.updateCityId = __bind(this.updateCityId, this);
					this.updateCivicNumber = __bind(this.updateCivicNumber, this);
					this.updateAptNumber = __bind(this.updateAptNumber, this);
					this.updateCity = __bind(this.updateCity, this);
					this.updateStreet = __bind(this.updateStreet, this);
					this.updatePostalCode = __bind(this.updatePostalCode, this); this.City = kb.observable(model, 'Address.City');
					this.Street = kb.observable(model, 'Address.Street');
					this.PostalCode = kb.observable(model, 'Address.PostalCode');
					this.AptNumber = kb.observable(model, 'Address.AptNumber');
					this.CivicNumber = kb.observable(model, 'Address.CivicNumber');
					this.AptNumber = kb.observable(model, 'Address.AptNumber');
					this.CityId = kb.observable(model, 'Address.CityId');
					this.NewCityName = kb.observable(model, 'Address.NewCityName');
					this.streetNameAutocomplete = new client.ClientStreetNameAutocompleteViewModel(this.CivicNumber, this.Street, this.PostalCode, this.CityId);
					this.civicNumberAutocomplete = new client.ClientCivicNumberAutocompleteViewModel(this.CivicNumber, this.Street, this.PostalCode);
					this.postalCodeAutocomplete = new client.ClientPostalCodeAutocompleteViewModel(this.CivicNumber, this.Street, this.PostalCode);
					model.on('change:Address.PostalCode', this.updatePostalCode);
					model.on('change:Address.Street', this.updateStreet);
					model.on('change:Address.City', this.updateCity);
					model.on('change:Address.CityId', this.updateCityId);
					model.on('change:Address.CivicNumber', this.updateCivicNumber);
					model.on('change:Address.AptNumber', this.updateAptNumber);
				}

				AddressViewModel.prototype.updatePostalCode = function (m, v) {
					return this.PostalCode(v);
				};

				AddressViewModel.prototype.updateStreet = function (m, v) {
					return this.Street(v);
				};

				AddressViewModel.prototype.updateCity = function (m, v) {
					return this.City(v);
				};

				AddressViewModel.prototype.updateAptNumber = function (m, v) {
					return this.AptNumber(v);
				};

				AddressViewModel.prototype.updateCivicNumber = function (m, v) {
					return this.CivicNumber(v);
				};

				AddressViewModel.prototype.updateCityId = function (m, v) {
					var cityId = "";
					var city = this.City();


					if ($("#selectCity")[0]) {
						if ($("#selectCity")[0].options[$("#selectCity")[0].selectedIndex].label === "Autre") {
							$($("#selectCity")[0].options).each(function (i, e) {
								if (e.label === city) {
									cityId = e.value;
								}
							});
							return this.CityId(cityId);
						} else {
							return this.CityId(v);
						}
					}
				};

				return AddressViewModel;

			})();

			ViewModel = (function (_super) {

				__extends(ViewModel, _super);

				function ViewModel(model, municipalities, categoriesModel, hubs, currentUserModel) {
					this.onError = __bind(this.onError, this);
					this.onSaved = __bind(this.onSaved, this);
					this.save = __bind(this.save, this);
					this.onCurrentUserLoaded = __bind(this.onCurrentUserLoaded, this);
					this.loadNew = __bind(this.loadNew, this);
					this.load = __bind(this.load, this);
					this.isNew = __bind(this.isNew, this);
					this.addNewOBNL = __bind(this.addNewOBNL, this);
					this.computeShowOBNLNumber = __bind(this.computeShowOBNLNumber, this);
					this.computeShowOrganizationName = __bind(this.computeShowOrganizationName, this);
					this.computeShowFirstNameLastName = __bind(this.computeShowFirstNameLastName, this);
					this.onMunicipalitiesReloaded = __bind(this.onMunicipalitiesReloaded, this);
					this.computeShowHubs = __bind(this.computeShowHubs, this);
					this.computeShowCustomCategoryName = __bind(this.computeShowCustomCategoryName, this); ViewModel.__super__.constructor.call(this, model);
					this.municipalitiesModel = municipalities;
					this.address = ko.observable(new AddressViewModel(model));
					this.hub = ko.observable(new HubViewModel(model));
					this.errors = ko.observableArray([]);
					this.OBNLNumbersInputed = ko.observableArray([]);
					this.categoriesModel = categoriesModel;
					this.currentUserModel = currentUserModel;
					this.Municipalities = ko.observableArray([]);
					municipalities.on('reloaded', this.onMunicipalitiesReloaded);
					this.onMunicipalitiesReloaded();
					this.Categories = kb.collectionObservable(categoriesModel.list);
					this.showFirstNameLastName = ko.computed(this.computeShowFirstNameLastName);
					this.hubs = kb.collectionObservable(hubs.hubs);
					this.showHub = ko.computed(this.computeShowHubs);
					this.showOrganizationName = ko.computed(this.computeShowOrganizationName);
					this.showCustomCategoryName = ko.computed(this.computeShowCustomCategoryName);
					this.showOBNLNumber = ko.computed(this.computeShowOBNLNumber);
					this.allowAddressCreation = kb.observable(model, 'AllowAddressCreation');
					this.NewCityName = ko.observable('');
				}

				ViewModel.prototype.computeShowCustomCategoryName = function () {
					return this.Category() === "Other";
				};

				ViewModel.prototype.computeShowHubs = function () {
					return this.hubs().length > 0;
				};

				ViewModel.prototype.onMunicipalitiesReloaded = function () {
					var m, _i, _len, _ref;
					this.Municipalities.removeAll();
					_ref = this.municipalitiesModel.municipalities.models;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						m = _ref[_i];
						this.Municipalities.push(m.attributes);
					}
					return this.Municipalities.push({
						Id: "",
						Name: "Autre"
					});
				};

				ViewModel.prototype.computeShowFirstNameLastName = function () {
					var item, _i, _len, _ref;
					_ref = this.Categories();
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						if (item.CategoryName() === this.Category()) {
							return item.ShowFirstNameLastName();
						}
					}
					return true;
				};

				ViewModel.prototype.computeShowOrganizationName = function () {
					var item, _i, _len, _ref;
					_ref = this.Categories();
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						if (item.CategoryName() === this.Category()) {
							return item.ShowOrganizationName();
						}
					}
					return false;
				};

				ViewModel.prototype.computeShowOBNLNumber = function () {
					return this.Category() === 'OBNL';
				};

				ViewModel.prototype.addNewOBNL = function () {
					if (this.OBNLNumber()) {
						if (this.OBNLNumbers() === null) this.OBNLNumbers([]);
						this.OBNLNumbers().push(this.OBNLNumber());
						this.OBNLNumbersInputed.push(this.OBNLNumber());
						return this.OBNLNumber('');
					}
				};

				ViewModel.prototype.isNew = function () {
					return this.model().isNew();
				};

				ViewModel.prototype.load = function (id) {
					return this.model().fetch({
						data: {
							id: id
						}
					});
				};

				ViewModel.prototype.loadNew = function (id) {
					return this.currentUserModel.fetch({
						success: this.onCurrentUserLoaded
					});
				};

				ViewModel.prototype.onCurrentUserLoaded = function () {
					this.model().set('Hub.Id', this.currentUserModel.get("HubId"));
					return this.model().set('Address.CityId', this.currentUserModel.get("DefaultCityId"));
				};

				ViewModel.prototype.save = function () {
					return this.model().save(null, {
						success: this.onSaved,
						error: this.onError
					});
				};

				ViewModel.prototype.save1 = function () {
					return this.model().save(null, {
						success: this.onSaved,
						error: this.onError
					});
				};

				ViewModel.prototype.onSaved = function (data) {
					eco.app.notifications.addNotification('msg-client-saved-successfully');
					return eco.app.router.navigate('client/index', {
						trigger: true
					});
				};

				ViewModel.prototype.onError = function (m, errors, data) {
					var confirmationText, item, _i, _len, _results;
					errors = $.parseJSON(errors.responseText);
					if (errors.length === 1 && errors[0].PropertyName === 'AllowAddressCreation') {
						confirmationText = kb.locale_manager.get("This address already exists in the database. Are you sure you want to create a customer at this address?");
						if (confirm(confirmationText)) {
							this.model().set('AllowAddressCreation', true);
							return this.save();
						}
					} else {
						this.errors.removeAll();
						_results = [];
						for (_i = 0, _len = errors.length; _i < _len; _i++) {
							item = errors[_i];
							_results.push(this.errors.push(item));
						}
						return _results;
					}
				};

				return ViewModel;

			})(kb.ViewModel);

			exports.createViewModel = function () {
				var categories, clientModule, currentUserModel, hubs, model, municipalities;
				clientModule = require('client');
				model = new clientModule.CandianModel();
				municipalities = new (require('municipality')).ListModel();
				municipalities.filter = function (item) {
					return item.Enabled;
				};
				municipalities.fetch({
					async: false
				});
				hubs = new (require('hub')).ListModel();
				hubs.fetch();
				categories = new clientModule.CategoriesModel();
				categories.fetch();
				currentUserModel = new (require('user')).CurrentUserModel();
				return new ViewModel(model, municipalities, categories, hubs, currentUserModel);
			};

		}).call(this);
	}, "client.inactive": function (exports, require, module) {
		(function () {
			var ClientRow, ViewModel, client, common,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			common = require('common');

			client = require('client');

			ClientRow = (function (_super) {

				__extends(ClientRow, _super);

				function ClientRow(model, options) {
					this.fold = __bind(this.fold, this);
					this.expand = __bind(this.expand, this); ClientRow.__super__.constructor.call(this, model, options);
					this.categoryStr = kb.observable(model, {
						key: 'Category',
						localizer: common.LocalizedStringLocalizer
					});
					this.Invoices = kb.collectionObservable(model.Invoices);
					this.ExcludedInvoices = kb.collectionObservable(model.ExcludedInvoices);
					this.IncludedInvoices = kb.collectionObservable(model.IncludedInvoices);
					this.expanded = ko.observable(false);
				}

				ClientRow.prototype.expand = function () {
					return this.expanded(true);
				};

				ClientRow.prototype.fold = function () {
					return this.expanded(false);
				};

				return ClientRow;

			})(kb.ViewModel);

			ViewModel = (function () {

				function ViewModel(model, hubs) {
					this.onCompletelyRemove = __bind(this.onCompletelyRemove, this);
					this.onActivate = __bind(this.onActivate, this);
					this.onEdit = __bind(this.onEdit, this);
					this.onShow = __bind(this.onShow, this);
					this.onNew = __bind(this.onNew, this);
					this.onNewCanadian = __bind(this.onNewCanadian, this);
					this.onPick = __bind(this.onPick, this);
					this.sort = __bind(this.sort, this);
					this.searchLetter = __bind(this.searchLetter, this);
					this.resetLetter = __bind(this.resetLetter, this);
					this.search = __bind(this.search, this);
					this.changePage = __bind(this.changePage, this);
					this.reset = __bind(this.reset, this);
					this.load = __bind(this.load, this);
					this.computeShowName = __bind(this.computeShowName, this);
					this.computeShowAddress = __bind(this.computeShowAddress, this);
					this.computeShowHubs = __bind(this.computeShowHubs, this);
					this.goToPage = __bind(this.goToPage, this); this.model = model;
					this.items = kb.collectionObservable(this.model.clients, {
						view_model: ClientRow
					});
					this.pageButtons = kb.collectionObservable(this.model.pageButtons);
					this.lastChange = kb.observable(model, 'filterLastChange');
					this.firstName = kb.observable(model, 'filterFirstName');
					this.lastName = kb.observable(model, 'filterLastName');
					this.citizenCard = kb.observable(model, 'filterCitizenCard');
					this.lastChange = kb.observable(model, 'filterLastChange');
					this.address = kb.observable(model, 'filterAddress');
					this.civicNumber = kb.observable(model, 'filterCivicNumber');
					this.postalCode = kb.observable(model, 'filterPostalCode');
					this.lastNameAutocomplete = new client.ClientLastNameAutocompleteViewModel(this.lastName);
					this.firstNameAutocomplete = new client.ClientFirstNameAutocompleteViewModel(this.firstName);
					this.streetNameAutocomplete = new client.ClientStreetNameAutocompleteViewModel(this.civicNumber, this.address);
					this.civicNumberAutocomplete = new client.ClientCivicNumberAutocompleteViewModel(this.civicNumber, this.address);
					this.civicCardAutocomplete = new client.ClientCivicCardAutocompleteViewModel(this.citizenCard);
					this.postalCodeAutocomplete = new client.ClientPostalCodeAutocompleteViewModel(this.postalCode);
					this.lastVisitFrom = kb.observable(model, 'filterLastVisitFrom');
					this.lastVisitTo = kb.observable(model, 'filterLastVisitTo');
					this.firstName.subscribe(this.resetLetter);
					this.lastName.subscribe(this.resetLetter);
					this.hubId = kb.observable(model, 'filterHubId');
					this.hubs = kb.collectionObservable(hubs.hubs);
					this.searchFirstLetter = kb.observable(model, 'filterFirstLetter');
					this.activeOnly = kb.observable(model, 'filterActive');
					this.inactiveOnly = kb.observable(model, 'filterInactive');
					this.verified = kb.observable(model, 'filterVerified');
					this.noverified = kb.observable(model, 'filterNoVerified');
					this.inactiveOnly(true);
					this.verified(true);
					this.noverified(false);
					this.sortDir = kb.observable(model, 'sortDir');
					this.sortBy = kb.observable(model, 'sortBy');
					this.searchType = kb.observable(model, 'searchType');
					this.filterType = kb.observable(model, 'filterType');
					this.term = kb.observable(model, 'term');
					this.searchfocus = ko.observable(true);
					this.mode = ko.observable('default');
					this.letters = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
					this.showName = ko.observable(true);
					this.showHub = ko.computed(this.computeShowHubs);
					this.showAddress = ko.observable(false);
					this.showFilterSelect = ko.observable(false);
					this.globalSettingsModel = new (require('globalsettings')).Model();
					this.globalSettingsModel.fetch({
						async: false
					});
					this.maxYearlyClientVisits = ko.observable(this.globalSettingsModel.get('MaxYearlyClientVisits'));
					this.page = kb.observable(model, 'page');
					this.pageCount = kb.observable(model, 'pageCount');
					this.throttledPage = ko.computed(this.page).extend({
						throttle: 400
					});
					this.pageSize = kb.observable(model, 'pageSize');
					this.computeShowName();
					this.computeShowAddress();
					this.searchType.subscribe(this.computeShowName);
					this.searchType.subscribe(this.computeShowAddress);
					this.throttledPage.subscribe(this.goToPage);
				}

				ViewModel.prototype.goToPage = function (val) {
					if (val && val > 1 && val < this.pageCount()) {
						return this.changePage({
							number: val
						});
					}
				};

				ViewModel.prototype.computeShowHubs = function () {
					return this.hubs().length > 0;
				};

				ViewModel.prototype.computeShowAddress = function () {
					return this.showAddress(this.searchType().toLowerCase() === "address");
				};

				ViewModel.prototype.computeShowName = function () {
					return this.showName(this.searchType().toLowerCase() === "name");
				};

				ViewModel.prototype.load = function () {
					this.model.changePage(1);
					return this.searchfocus = ko.observable(true);
				};

				ViewModel.prototype.reset = function () {
					this.model.clients.reset();
					return this.model.pageButtons.reset();
				};

				ViewModel.prototype.changePage = function (vm) {
					return this.model.changePage(vm.number);
				};

				ViewModel.prototype.search = function () {
					return this.model.search();
				};

				ViewModel.prototype.resetLetter = function () {
					return this.searchFirstLetter('');
				};

				ViewModel.prototype.searchLetter = function (ltr) {
					this.firstName('');
					this.lastName('');
					this.searchFirstLetter(ltr);
					return this.search();
				};

				ViewModel.prototype.sort = function (orderBy) {
					if (this.sortBy() !== orderBy) {
						this.sortBy(orderBy);
						this.sortDir('Asc');
					} else {
						this.sortDir(this.sortDir() === 'Asc' ? 'Desc' : 'Asc');
					}
					return this.load();
				};

				ViewModel.prototype.onPick = function (vm) {
					return true;
				};

				ViewModel.prototype.onNew = function () {
					return eco.app.router.navigate('client/new', {
						trigger: true
					});
				};

				ViewModel.prototype.onNewCanadian = function () {
					return eco.app.router.navigate('client/newcanadian', {
						trigger: true
					});
				};

				ViewModel.prototype.onCancel = null;

				ViewModel.prototype.onShow = function (vm, e) {
					return eco.app.router.navigate('client/show/' + vm.Id(), {
						trigger: true
					});
				};

				ViewModel.prototype.onEdit = function (vm) {
					return eco.app.router.navigate('client/edit/' + vm.Id(), {
						trigger: true
					});
				};

				ViewModel.prototype.onActivate = function (vm) {
					if (!confirm(kb.locale_manager.get("Voulez-vous rétablir ce client?"))) {
						return;
					}
					vm.model().set({
						'Status': 0,
						'UpdateOnlyStatus': true
					});
					vm.model().save();
					return vm.model().collection.remove(vm.model());
				};

				ViewModel.prototype.onCompletelyRemove = function (vm) {
					if (!confirm(kb.locale_manager.get("Voulez-vous supprimer complètement ce client?\nATTENTION! Cette opération est IRRÉVERSIBLE!"))) {
						return;
					}
					return vm.model().destroy();
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var hubs;
				if (exports.vm == null) {
					exports.model = new client.ClientList();
					hubs = new (require('hub')).ListModel();
					hubs.fetch();
					exports.vm = new ViewModel(exports.model, hubs);
				}
				return exports.vm;
			};

		}).call(this);
	}, "client.list": function (exports, require, module) {
		(function () {
			var ClientRow, ViewModel, client, common,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			common = require('common');

			client = require('client');

			ClientRow = (function (_super) {

				__extends(ClientRow, _super);

				function ClientRow(model, options) {
					this.fold = __bind(this.fold, this);
					this.expand = __bind(this.expand, this); ClientRow.__super__.constructor.call(this, model, options);
					this.categoryStr = kb.observable(model, {
						key: 'Category',
						localizer: common.LocalizedStringLocalizer
					});
					this.Invoices = kb.collectionObservable(model.Invoices);
					this.OBNLReinvestments = kb.collectionObservable(model.OBNLReinvestments);
					this.IncludedOBNLReinvestments = kb.collectionObservable(model.IncludedOBNLReinvestments);
					this.ExcludedInvoices = kb.collectionObservable(model.ExcludedInvoices);
					this.IncludedInvoices = kb.collectionObservable(model.IncludedInvoices);
					this.expanded = ko.observable(false);
					this.isFromAnotherHub = ko.observable(true);
				}

				ClientRow.prototype.expand = function () {
					return this.expanded(true);
				};

				ClientRow.prototype.fold = function () {
					return this.expanded(false);
				};

				return ClientRow;

			})(kb.ViewModel);

			ViewModel = (function () {

				function ViewModel(model, hubs, selectedHub) {
					this.onCompletelyRemove = __bind(this.onCompletelyRemove, this);
					this.onActivate = __bind(this.onActivate, this);
					this.onRemove = __bind(this.onRemove, this);
					this.onEdit = __bind(this.onEdit, this);
					this.onShow = __bind(this.onShow, this);
					this.onNew = __bind(this.onNew, this);
					this.onNewCanadian = __bind(this.onNewCanadian, this);
					this.onPick = __bind(this.onPick, this);
					this.sort = __bind(this.sort, this);
					this.searchLetter = __bind(this.searchLetter, this);
					this.resetLetter = __bind(this.resetLetter, this);
					this.search = __bind(this.search, this);
					this.changePage = __bind(this.changePage, this);
					this.reset = __bind(this.reset, this);
					this.load = __bind(this.load, this);
					this.computeShowName = __bind(this.computeShowName, this);
					this.updateFilter = __bind(this.updateFilter, this);
					this.isOBNL = __bind(this.isOBNL, this);
					this.goToPage = __bind(this.goToPage, this);
					this.computeShowAddress = __bind(this.computeShowAddress, this);
					this.computeShowHubs = __bind(this.computeShowHubs, this); this.model = model;
					this.items = kb.collectionObservable(this.model.clients, {
						view_model: ClientRow
					});
					this.pageButtons = kb.collectionObservable(this.model.pageButtons);
					this.lastChange = kb.observable(model, 'filterLastChange');
					this.firstName = kb.observable(model, 'filterFirstName');
					this.lastName = kb.observable(model, 'filterLastName');
					this.citizenCard = kb.observable(model, 'filterCitizenCard');
					this.lastChange = kb.observable(model, 'filterLastChange');
					this.address = kb.observable(model, 'filterAddress');
					this.civicNumber = kb.observable(model, 'filterCivicNumber');
					this.postalCode = kb.observable(model, 'filterPostalCode');
					this.OBNLNumber = kb.observable(model, 'filterOBNLNumber');
					this.hubId = kb.observable(model, 'filterHubId');
					this.lastNameAutocomplete = new client.ClientLastNameAutocompleteViewModel(this.lastName);
					this.firstNameAutocomplete = new client.ClientFirstNameAutocompleteViewModel(this.firstName);
					this.streetNameAutocomplete = new client.ClientStreetNameAutocompleteViewModel(this.civicNumber, this.address, null, null, this.hubId);
					this.civicNumberAutocomplete = new client.ClientCivicNumberAutocompleteViewModel(this.civicNumber, this.address);
					this.civicCardAutocomplete = new client.ClientCivicCardAutocompleteViewModel(this.citizenCard);
					this.postalCodeAutocomplete = new client.ClientPostalCodeAutocompleteViewModel(this.postalCode);
					this.obnlNumberAutocomplete = new (require('obnlreinvestment')).OBNLNumberAutocompleteViewModel(this.OBNLNumber);
					this.lastVisitFrom = kb.observable(model, 'filterLastVisitFrom');
					this.lastVisitTo = kb.observable(model, 'filterLastVisitTo');
					this.firstName.subscribe(this.resetLetter);
					this.lastName.subscribe(this.resetLetter);
					if (selectedHub) model.set('filterHubId', selectedHub.get('Id'));
					this.categoryFilter = kb.observable(model, 'filterCategory');
					this.hubs = kb.collectionObservable(hubs.hubs);
					this.searchFirstLetter = kb.observable(model, 'filterFirstLetter');
					this.activeOnly = kb.observable(model, 'filterActive');
					this.inactiveOnly = kb.observable(model, 'filterInactive');
					this.verified = kb.observable(model, 'filterVerified');
					this.noverified = kb.observable(model, 'filterNoVerified');
					this.sortDir = kb.observable(model, 'sortDir');
					this.sortBy = kb.observable(model, 'sortBy');
					this.searchType = kb.observable(model, 'searchType');
					this.page = kb.observable(model, 'page');
					this.pageCount = kb.observable(model, 'pageCount');
					this.throttledPage = ko.computed(this.page).extend({
						throttle: 400
					});
					this.pageSize = kb.observable(model, 'pageSize');
					this.filterType = kb.observable(model, 'filterType');
					this.term = kb.observable(model, 'term');
					this.searchfocus = ko.observable(true);
					this.mode = ko.observable('default');
					this.letters = ['Tous', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
					this.showName = ko.observable(true);
					this.showHub = ko.computed(this.computeShowHubs);
					this.showAddress = ko.observable(false);
					this.showFilterSelect = ko.observable(true);
					this.globalSettingsModel = new (require('globalsettings')).Model();
					this.globalSettingsModel.fetch({
						async: false
					});
					this.maxYearlyClientVisits = ko.observable(this.globalSettingsModel.get('MaxYearlyClientVisits'));
					this.computeShowName();
					this.computeShowAddress();
					this.searchType.subscribe(this.computeShowName);
					this.searchType.subscribe(this.computeShowAddress);
					this.filterType.subscribe(this.updateFilter);
					this.throttledPage.subscribe(this.goToPage);
				}

				ViewModel.prototype.computeShowHubs = function () {
					return this.hubs().length > 0;
				};

				ViewModel.prototype.computeShowAddress = function () {
					return this.showAddress(this.searchType().toLowerCase() === "address");
				};

				ViewModel.prototype.goToPage = function (val) {
					if (val && val > 1 && val < this.pageCount()) {
						return this.changePage({
							number: val
						});
					}
				};

				ViewModel.prototype.isOBNL = function (item) {
					if (item.toLowerCase() === 'obnl') {
						return true;
					} else {
						return false;
					}
				};

				ViewModel.prototype.updateFilter = function () {
					switch (this.filterType().toLowerCase()) {
						case "active":
							this.verified(true);
							this.activeOnly(true);
							return this.inactiveOnly(false);
						case "inactive":
							this.verified(true);
							this.activeOnly(false);
							return this.inactiveOnly(true);
						case "verified":
							this.verified(true);
							return this.verified(true);
						case "noverified":
							this.noverified(true);
							return this.verified(false);
						default:
							this.activeOnly(false);
							this.verified(true);
							return this.inactiveOnly(false);
					}
				};

				ViewModel.prototype.computeShowName = function () {
					return this.showName(this.searchType().toLowerCase() === "name");
				};

				ViewModel.prototype.load = function () {
					this.model.set({
						filterOBNLNumber: '',
						filterCategory: 'All'
					});
					this.model.changePage(1);
					return this.searchfocus = ko.observable(true);
				};

				ViewModel.prototype.reset = function () {
					this.model.clients.reset();
					return this.model.pageButtons.reset();
				};

				ViewModel.prototype.changePage = function (vm) {
					return this.model.changePage(vm.number);
				};

				ViewModel.prototype.search = function () {
					return this.model.search();
				};

				ViewModel.prototype.resetLetter = function () {
					return this.searchFirstLetter('');
				};

				ViewModel.prototype.searchLetter = function (ltr) {
					this.firstName('');
					this.lastName('');
					this.searchFirstLetter(ltr === 'Tous' ? '' : ltr);
					return this.search();
				};

				ViewModel.prototype.sort = function (orderBy) {
					if (this.sortBy() !== orderBy) {
						this.sortBy(orderBy);
						this.sortDir('Asc');
					} else {
						this.sortDir(this.sortDir() === 'Asc' ? 'Desc' : 'Asc');
					}
					return this.load();
				};

				ViewModel.prototype.onPick = function (vm) {
					return true;
				};

				ViewModel.prototype.onNew = function () {
					return eco.app.router.navigate('client/new', {
						trigger: true
					});
				};

				ViewModel.prototype.onNewCanadian = function () {
					return eco.app.router.navigate('client/newcanadian', {
						trigger: true
					});
				};

				ViewModel.prototype.onCancel = null;

				ViewModel.prototype.onShow = function (vm, e) {
					return eco.app.router.navigate('client/show/' + vm.Id(), {
						trigger: true
					});
				};

				ViewModel.prototype.onEdit = function (vm) {
					return eco.app.router.navigate('client/edit/' + vm.Id(), {
						trigger: true
					});
				};

				ViewModel.prototype.onRemove = function (vm) {
					if (!confirm(kb.locale_manager.get("Voulez-vous supprimer ce client?"))) {
						return;
					}
					vm.model().set({
						'Status': 1,
						'UpdateOnlyStatus': true
					});
					vm.model().save();
					return vm.model().collection.remove(vm.model());
				};

				ViewModel.prototype.onActivate = function (vm) {
					if (!confirm(kb.locale_manager.get("Voulez-vous rétablir ce client?"))) {
						return;
					}
					vm.model().set({
						'Status': 0,
						'UpdateOnlyStatus': true
					});
					vm.model().save();
					return vm.model().collection.remove(vm.model());
				};

				ViewModel.prototype.onCompletelyRemove = function (vm) {
					if (!confirm(kb.locale_manager.get("Voulez-vous supprimer complètement ce client?\nATTENTION! Cette opération est IRRÉVERSIBLE!"))) {
						return;
					}
					return vm.model().destroy();
				};

				return ViewModel;

			})();

			exports.createViewModel = function (hub) {
				var hubs;
				exports.model = new client.ClientList();
				hubs = new (require('hub')).ListModel();
				hubs.fetch({
					async: false
				});
				if (!hub) {
					hub = new (require('hub')).Model();
					hub.fetchCurrent();
				}
				exports.vm = new ViewModel(exports.model, hubs, hub);
				return exports.vm;
			};

		}).call(this);
	}, "client.list1": function (exports, require, module) {
		(function () {
			var ClientRow, ViewModel, client, common,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			common = require('common');

			client = require('client');

			ClientRow = (function (_super) {

				__extends(ClientRow, _super);

				function ClientRow(model, options) {
					this.fold = __bind(this.fold, this);
					this.expand = __bind(this.expand, this); ClientRow.__super__.constructor.call(this, model, options);
					this.categoryStr = kb.observable(model, {
						key: 'Category',
						localizer: common.LocalizedStringLocalizer
					});
					this.Invoices = kb.collectionObservable(model.Invoices);
					this.OBNLReinvestments = kb.collectionObservable(model.OBNLReinvestments);
					this.IncludedOBNLReinvestments = kb.collectionObservable(model.IncludedOBNLReinvestments);
					this.ExcludedInvoices = kb.collectionObservable(model.ExcludedInvoices);
					this.IncludedInvoices = kb.collectionObservable(model.IncludedInvoices);
					this.expanded = ko.observable(false);
					this.isFromAnotherHub = ko.observable(true);
				}

				ClientRow.prototype.expand = function () {
					return this.expanded(true);
				};

				ClientRow.prototype.fold = function () {
					return this.expanded(false);
				};

				return ClientRow;

			})(kb.ViewModel);

			ViewModel = (function () {

				function ViewModel(model, hubs, selectedHub) {
					this.onCompletelyRemove = __bind(this.onCompletelyRemove, this);
					this.onActivate = __bind(this.onActivate, this);
					this.onRemove = __bind(this.onRemove, this);
					this.onEdit = __bind(this.onEdit, this);
					this.onShow = __bind(this.onShow, this);
					this.onNew = __bind(this.onNew, this);
					this.onNewCanadian = __bind(this.onNewCanadian, this);
					this.onPick = __bind(this.onPick, this);
					this.sort = __bind(this.sort, this);
					this.searchLetter = __bind(this.searchLetter, this);
					this.resetLetter = __bind(this.resetLetter, this);
					this.search = __bind(this.search, this);
					this.changePage = __bind(this.changePage, this);
					this.reset = __bind(this.reset, this);
					this.load = __bind(this.load, this);
					this.computeShowName = __bind(this.computeShowName, this);
					this.updateFilter = __bind(this.updateFilter, this);
					this.isOBNL = __bind(this.isOBNL, this);
					this.goToPage = __bind(this.goToPage, this);
					this.computeShowAddress = __bind(this.computeShowAddress, this);
					this.computeShowHubs = __bind(this.computeShowHubs, this); this.model = model;
					this.items = kb.collectionObservable(this.model.clients, {
						view_model: ClientRow
					});
					this.pageButtons = kb.collectionObservable(this.model.pageButtons);
					this.lastChange = kb.observable(model, 'filterLastChange');
					this.firstName = kb.observable(model, 'filterFirstName');
					this.lastName = kb.observable(model, 'filterLastName');
					this.citizenCard = kb.observable(model, 'filterCitizenCard');
					this.lastChange = kb.observable(model, 'filterLastChange');
					this.address = kb.observable(model, 'filterAddress');
					this.civicNumber = kb.observable(model, 'filterCivicNumber');
					this.postalCode = kb.observable(model, 'filterPostalCode');
					this.OBNLNumber = kb.observable(model, 'filterOBNLNumber');
					this.hubId = kb.observable(model, 'filterHubId');
					this.lastNameAutocomplete = new client.ClientLastNameAutocompleteViewModel1(this.lastName);
					this.firstNameAutocomplete = new client.ClientFirstNameAutocompleteViewModel1(this.firstName);
					this.streetNameAutocomplete = new client.ClientStreetNameAutocompleteViewModel1(this.civicNumber, this.address, null, null, this.hubId);
					this.civicNumberAutocomplete = new client.ClientCivicNumberAutocompleteViewModel1(this.civicNumber, this.address);
					this.civicCardAutocomplete = new client.ClientCivicCardAutocompleteViewModel1(this.citizenCard);
					this.postalCodeAutocomplete = new client.ClientPostalCodeAutocompleteViewModel1(this.postalCode);
					this.obnlNumberAutocomplete = new (require('obnlreinvestment')).OBNLNumberAutocompleteViewModel(this.OBNLNumber);
					this.lastVisitFrom = kb.observable(model, 'filterLastVisitFrom');
					this.lastVisitTo = kb.observable(model, 'filterLastVisitTo');
					this.firstName.subscribe(this.resetLetter);
					this.lastName.subscribe(this.resetLetter);
					if (selectedHub) model.set('filterHubId', selectedHub.get('Id'));
					this.categoryFilter = kb.observable(model, 'filterCategory');
					this.hubs = kb.collectionObservable(hubs.hubs);
					this.searchFirstLetter = kb.observable(model, 'filterFirstLetter');
					this.activeOnly = kb.observable(model, 'filterActive');
					this.inactiveOnly = kb.observable(model, 'filterInactive');
					this.verified = kb.observable(model, 'filterVerified');
					this.noverified = kb.observable(model, 'filterNoVerified');
					this.sortDir = kb.observable(model, 'sortDir');
					this.sortBy = kb.observable(model, 'sortBy');
					this.searchType = kb.observable(model, 'searchType');
					this.page = kb.observable(model, 'page');
					this.pageCount = kb.observable(model, 'pageCount');
					this.throttledPage = ko.computed(this.page).extend({
						throttle: 400
					});
					this.pageSize = kb.observable(model, 'pageSize');
					this.filterType = kb.observable(model, 'filterType');
					this.term = kb.observable(model, 'term');
					this.searchfocus = ko.observable(true);
					this.mode = ko.observable('default');
					this.letters = ['Tous', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
					this.showName = ko.observable(true);
					this.showHub = ko.computed(this.computeShowHubs);
					this.showAddress = ko.observable(false);
					this.showFilterSelect = ko.observable(true);
					this.globalSettingsModel = new (require('globalsettings')).Model();
					this.globalSettingsModel.fetch({
						async: false
					});
					this.maxYearlyClientVisits = ko.observable(this.globalSettingsModel.get('MaxYearlyClientVisits'));
					this.computeShowName();
					this.computeShowAddress();
					this.searchType.subscribe(this.computeShowName);
					this.searchType.subscribe(this.computeShowAddress);
					this.filterType.subscribe(this.updateFilter);
					this.throttledPage.subscribe(this.goToPage);
				}

				ViewModel.prototype.computeShowHubs = function () {
					return this.hubs().length > 0;
				};

				ViewModel.prototype.computeShowAddress = function () {
					return this.showAddress(this.searchType().toLowerCase() === "address");
				};

				ViewModel.prototype.goToPage = function (val) {
					if (val && val > 1 && val < this.pageCount()) {
						return this.changePage({
							number: val
						});
					}
				};

				ViewModel.prototype.isOBNL = function (item) {
					if (item.toLowerCase() === 'obnl') {
						return true;
					} else {
						return false;
					}
				};

				ViewModel.prototype.updateFilter = function () {
					switch (this.filterType().toLowerCase()) {
						case "active":
							this.verified(true);
							this.activeOnly(true);
							return this.inactiveOnly(false);
						case "inactive":
							this.verified(true);
							this.activeOnly(false);
							return this.inactiveOnly(true);
						case "verified":
							this.verified(true);
							return this.verified(true);
						case "noverified":
							this.noverified(true);
							return this.verified(false);
						default:
							this.activeOnly(false);
							this.verified(true);
							return this.inactiveOnly(false);
					}
				};

				ViewModel.prototype.computeShowName = function () {
					return this.showName(this.searchType().toLowerCase() === "name");
				};

				ViewModel.prototype.load = function () {
					this.model.set({
						filterOBNLNumber: '',
						filterCategory: 'All'
					});
					this.model.changePage(1);
					return this.searchfocus = ko.observable(true);
				};

				ViewModel.prototype.reset = function () {
					this.model.clients.reset();
					return this.model.pageButtons.reset();
				};

				ViewModel.prototype.changePage = function (vm) {
					return this.model.changePage(vm.number);
				};

				ViewModel.prototype.search = function () {
					return this.model.search();
				};

				ViewModel.prototype.resetLetter = function () {
					return this.searchFirstLetter('');
				};

				ViewModel.prototype.searchLetter = function (ltr) {
					this.firstName('');
					this.lastName('');
					this.searchFirstLetter(ltr === 'Tous' ? '' : ltr);
					return this.search();
				};

				ViewModel.prototype.sort = function (orderBy) {
					if (this.sortBy() !== orderBy) {
						this.sortBy(orderBy);
						this.sortDir('Asc');
					} else {
						this.sortDir(this.sortDir() === 'Asc' ? 'Desc' : 'Asc');
					}
					return this.load();
				};

				ViewModel.prototype.onPick = function (vm) {
					return true;
				};

				ViewModel.prototype.onNew = function () {
					return eco.app.router.navigate('client/new', {
						trigger: true
					});
				};

				ViewModel.prototype.onNewCanadian = function () {
					return eco.app.router.navigate('client/newcanadian', {
						trigger: true
					});
				};

				ViewModel.prototype.onCancel = null;

				ViewModel.prototype.onShow = function (vm, e) {
					return eco.app.router.navigate('client/show/' + vm.Id(), {
						trigger: true
					});
				};

				ViewModel.prototype.onEdit = function (vm) {
					return eco.app.router.navigate('client/edit/' + vm.Id(), {
						trigger: true
					});
				};

				ViewModel.prototype.onRemove = function (vm) {
					if (!confirm(kb.locale_manager.get("Voulez-vous supprimer ce client?"))) {
						return;
					}
					vm.model().set({
						'Status': 1,
						'UpdateOnlyStatus': true
					});
					vm.model().save();
					return vm.model().collection.remove(vm.model());
				};

				ViewModel.prototype.onActivate = function (vm) {
					if (!confirm(kb.locale_manager.get("Voulez-vous rétablir ce client?"))) {
						return;
					}
					vm.model().set({
						'Status': 0,
						'UpdateOnlyStatus': true
					});
					vm.model().save();
					return vm.model().collection.remove(vm.model());
				};

				ViewModel.prototype.onCompletelyRemove = function (vm) {
					if (!confirm(kb.locale_manager.get("Voulez-vous supprimer complètement ce client?\nATTENTION! Cette opération est IRRÉVERSIBLE!"))) {
						return;
					}
					return vm.model().destroy();
				};

				return ViewModel;

			})();

			exports.createViewModel = function (hub) {
				var hubs;
				exports.model = new client.ClientList();
				hubs = new (require('hub')).ListModel();
				hubs.fetch({
					async: false
				});
				if (!hub) {
					hub = new (require('hub')).Model();
					hub.fetchCurrent();
				}
				exports.vm = new ViewModel(exports.model, hubs, hub);
				return exports.vm;
			};

		}).call(this);
	}, "client.merger": function (exports, require, module) {
		(function () {
			var ClientMergerModel, ClientMergerWorkflow, ClientViewModel, ViewModel, client,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			client = require('client');

			ClientViewModel = (function (_super) {

				__extends(ClientViewModel, _super);

				function ClientViewModel(model) {
					this.showFirstNameLastName = __bind(this.showFirstNameLastName, this);
					this.computeShowContactWarning = __bind(this.computeShowContactWarning, this);
					this.computeShowComment = __bind(this.computeShowComment, this);
					this.computeShowUnverifiedWarning = __bind(this.computeShowUnverifiedWarning, this);
					this.computeFullName = __bind(this.computeFullName, this); ClientViewModel.__super__.constructor.call(this, model);
					this.model = model;
					this.categories = new (require('client')).CategoriesModel();
					this.categories.fetch();
					this.fullName = ko.computed(this.computeFullName);
					this.invoices = kb.collectionObservable(this.model.Invoices);
					this.showContactWarning = ko.computed(this.computeShowContactWarning);
					this.showUnverifiedWarning = ko.computed(this.computeShowUnverifiedWarning);
					this.showComment = ko.computed(this.computeShowComment);
				}

				ClientViewModel.prototype.computeFullName = function () {
					return this.FirstName() + ' ' + this.LastName();
				};

				ClientViewModel.prototype.computeShowUnverifiedWarning = function () {
					return !this.Verified();
				};

				ClientViewModel.prototype.computeShowComment = function () {
					return this.Comments() !== null && this.Comments() !== '';
				};

				ClientViewModel.prototype.computeShowContactWarning = function () {
					var email, phone;
					email = this.Email();
					phone = this.PhoneNumber();
					return email === null || email.length < 1 || phone === null || phone.length < 0;
				};

				ClientViewModel.prototype.showFirstNameLastName = function () {
					return this.categories.showFirstNameLastName(this.get('Category'));
				};

				return ClientViewModel;

			})(Knockback.ViewModel);

			ClientMergerModel = (function (_super) {

				__extends(ClientMergerModel, _super);

				function ClientMergerModel() {
					this.resetMergeDest = __bind(this.resetMergeDest, this);
					this.save = __bind(this.save, this);
					this.removeMergeSource = __bind(this.removeMergeSource, this);
					this.addMergeSource = __bind(this.addMergeSource, this);
					this.selectMergeDest = __bind(this.selectMergeDest, this); this.url = 'client/merge';
					ClientMergerModel.__super__.constructor.call(this, {
						MergeDest: null,
						MergeSources: []
					});
				}

				ClientMergerModel.prototype.selectMergeDest = function (client) {
					return this.set({
						MergeDest: client.Id()
					});
				};

				ClientMergerModel.prototype.addMergeSource = function (client) {
					var curMergeSources;
					curMergeSources = this.get('MergeSources');
					if (!~curMergeSources.indexOf(client.Id())) {
						curMergeSources.push(client.Id());
						client.client_merger_clientWasPicked(true);
						return true;
					}
					return false;
				};

				ClientMergerModel.prototype.removeMergeSource = function (client) {
					var curMergeSources, index;
					curMergeSources = this.get('MergeSources');
					index = curMergeSources.indexOf(client.Id());
					if (~index) {
						curMergeSources.splice(index, 1);
						client.client_merger_clientWasPicked(false);
					}
					return index;
				};

				ClientMergerModel.prototype.save = function () {
					var mergeSourcesString;
					mergeSourcesString = this.get('MergeSources').join(',');
					this.set({
						MergeSourcesStr: mergeSourcesString
					});
					return ClientMergerModel.__super__.save.apply(this, arguments).save;
				};

				ClientMergerModel.prototype.resetMergeDest = function (client) {
					if (client.Id() === this.get('MergeDest')) {
						this.set({
							MergeDest: null
						});
						return true;
					}
					return false;
				};

				return ClientMergerModel;

			})(Backbone.Model);

			ViewModel = (function (_super) {

				__extends(ViewModel, _super);

				function ViewModel(model, clientsList) {
					this.removeClientNameFromHeaderElement = __bind(this.removeClientNameFromHeaderElement, this);
					this.addClientNameToHeaderElement = __bind(this.addClientNameToHeaderElement, this);
					this.changeDest = __bind(this.changeDest, this);
					this.onRemoveSrcPick = __bind(this.onRemoveSrcPick, this);
					this.onError = __bind(this.onError, this);
					this.onMerged = __bind(this.onMerged, this);
					this.merge = __bind(this.merge, this);
					this.load = __bind(this.load, this); this.model = model;
					ViewModel.__super__.constructor.call(this, model);
					this.errors = ko.observableArray([]);
					this.clientsList = clientsList;
					this.mergeDest = ko.observable(null);
					this.mergeSources = ko.observableArray();
					this.showDestList = ko.observable(true);
				}

				ViewModel.prototype.load = function () {
					return this.model().fetch();
				};

				ViewModel.prototype.merge = function () {
					if (!this.mergeDest() || !this.mergeSources().length) {
						alert("Se il vous plaît, sélectionnez les deux fusionnent sources et fusionner destination de continuer.");
						return;
					}
					if (!confirm("Attention! L'opération est irréversible! Etes-vous sûr de vouloir continuer?")) {
						return;
					}
					$("#global-loading-fade").modal('show');
					return this.model().save(null, {
						success: this.onMerged,
						error: this.onError
					});
				};

				ViewModel.prototype.onMerged = function () {
					var setTimeoutCallback;
					setTimeoutCallback = function () {
						return $("#global-loading-fade").modal('hide');
					};
					setTimeout(setTimeoutCallback, 500);
					eco.app.notifications.addNotification('msg-client-merger-run-successfully');
					return eco.app.router.navigate('client/show/' + this.model().get('MergeDest'), {
						trigger: true
					});
				};

				ViewModel.prototype.onError = function (m, errors, data) {
					var item, setTimeoutCallback, _i, _len, _results;
					setTimeoutCallback = function () {
						return $("#global-loading-fade").modal('hide');
					};
					setTimeout(setTimeoutCallback, 500);
					errors = $.parseJSON(errors.responseText);
					this.errors.removeAll();
					eco.app.notifications.removeNotification('msg-client-merger-run-successfully');
					_results = [];
					for (_i = 0, _len = errors.length; _i < _len; _i++) {
						item = errors[_i];
						_results.push(this.errors.push(item));
					}
					return _results;
				};

				ViewModel.prototype.onRemoveSrcPick = function (client) {
					var index;
					index = this.model().removeMergeSource(client);
					if (~index) {
						this.mergeSources.splice(index, 1);
						return this.removeClientNameFromHeaderElement($('#merge-sources-names'), client);
					}
				};

				ViewModel.prototype.changeDest = function (client) {
					return this.showDestList(true);
				};

				ViewModel.prototype.addClientNameToHeaderElement = function ($element, client, replace) {
					var clientFullName, elementText;
					elementText = $element.text();
					clientFullName = client.LastName() + ' ' + client.FirstName();
					if (!replace) {
						return $element.text(elementText && elementText !== '-' ? elementText + ', ' + clientFullName : clientFullName);
					} else {
						return $element.text(clientFullName);
					}
				};

				ViewModel.prototype.removeClientNameFromHeaderElement = function ($element, client) {
					var clientFullName, elementText, removalRegExp;
					elementText = $element.text();
					clientFullName = client.LastName() + ' ' + client.FirstName();
					removalRegExp = new RegExp('(,\\s)?' + clientFullName.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'));
					elementText = elementText.replace(removalRegExp, '');
					elementText = elementText.replace(/^,\s/g, '');
					if (!elementText) elementText = '-';
					return $element.text(elementText);
				};

				return ViewModel;

			})(Knockback.ViewModel);

			ClientMergerWorkflow = (function () {

				function ClientMergerWorkflow() {
					this.showClientList = __bind(this.showClientList, this);
					this.runMerger = __bind(this.runMerger, this); this.formViewModel = null;
					this.merger = null;
				}

				ClientMergerWorkflow.prototype.runMerger = function () {
					var clientsList,
						_this = this;
					this.merger = new ClientMergerModel();
					clientsList = this.showClientList();
					clientsList.model.clients.bind('add', function (item) {
						return item.set({
							client_merger_clientWasPicked: false
						});
					});
					clientsList.reset();
					return this.formViewModel = new ViewModel(this.merger, clientsList);
				};

				ClientMergerWorkflow.prototype.showClientList = function (setup) {
					var clientListVm,
						_this = this;
					if (setup == null) setup = null;
					clientListVm = new (require('client.list')).createViewModel();
					clientListVm.mode('pick');
					clientListVm.model.set({
						noCommercial: true
					});
					clientListVm.onDestPick = function (client) {
						_this.merger.selectMergeDest(client);
						_this.formViewModel.mergeDest(new ClientViewModel(client.model()));
						_this.formViewModel.showDestList(false);
						return _this.formViewModel.addClientNameToHeaderElement($('#merge-dest-names'), client, true);
					};
					clientListVm.onSrcPick = function (client) {
						if (_this.merger.addMergeSource(client)) {
							_this.formViewModel.mergeSources.push(client);
							if (_this.merger.resetMergeDest(client)) {
								_this.formViewModel.changeDest();
								_this.formViewModel.removeClientNameFromHeaderElement($('#merge-dest-names'), client);
							}
							return _this.formViewModel.addClientNameToHeaderElement($('#merge-sources-names'), client);
						}
					};
					if (setup != null) setup(clientListVm);
					return clientListVm;
				};

				return ClientMergerWorkflow;

			})();

			exports.createViewModel = function () {
				var workflow;
				workflow = new ClientMergerWorkflow();
				return workflow.runMerger();
			};

		}).call(this);
	}, "client.show": function (exports, require, module) {
		(function () {
			var AddressViewModel, HubViewModel, ViewModel,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			HubViewModel = (function () {

				function HubViewModel(model) {
					this.updateId = __bind(this.updateId, this);
					this.updateName = __bind(this.updateName, this); this.Name = kb.observable(model, 'Hub.Name');
					this.Id = kb.observable(model, 'Hub.Id');
				}

				HubViewModel.prototype.updateName = function (m, v) {
					return this.Name(v);
				};

				HubViewModel.prototype.updateId = function (m, v) {
					return this.Id(v);
				};

				return HubViewModel;

			})();

			AddressViewModel = (function () {

				function AddressViewModel(model) {
					this.updateAptNumber = __bind(this.updateAptNumber, this);
					this.updateCityId = __bind(this.updateCityId, this);
					this.updateCivicNumber = __bind(this.updateCivicNumber, this);
					this.updateCity = __bind(this.updateCity, this);
					this.updateStreet = __bind(this.updateStreet, this);
					this.updatePostalCode = __bind(this.updatePostalCode, this); this.City = kb.observable(model, 'Address.City');
					this.Street = kb.observable(model, 'Address.Street');
					this.PostalCode = kb.observable(model, 'Address.PostalCode');
					this.CivicNumber = kb.observable(model, 'Address.CivicNumber');
					this.AptNumber = kb.observable(model, 'Address.AptNumber');
					this.CityId = kb.observable(model, 'Address.CityId');
					model.on('change:Address.PostalCode', this.updatePostalCode);
					model.on('change:Address.Street', this.updateStreet);
					model.on('change:Address.City', this.updateCity);
					model.on('change:Address.CityId', this.updateCityId);
					model.on('change:Address.CivicNumber', this.updateCivicNumber);
					model.on('change:Address.AptNumber', this.updateAptNumber);
				}

				AddressViewModel.prototype.updatePostalCode = function (m, v) {
					return this.PostalCode(v);
				};

				AddressViewModel.prototype.updateStreet = function (m, v) {
					return this.Street(v);
				};

				AddressViewModel.prototype.updateCity = function (m, v) {
					return this.City(v);
				};

				AddressViewModel.prototype.updateCivicNumber = function (m, v) {
					return this.CivicNumber(v);
				};

                AddressViewModel.prototype.updateCityId = function (m, v) {
                    return this.CityId(v);
				};

				AddressViewModel.prototype.updateAptNumber = function (m, v) {
					return this.AptNumber(v);
				};

				return AddressViewModel;

			})();

			ViewModel = (function (_super) {

				__extends(ViewModel, _super);

				function ViewModel(model, invoices, obnlReinvestments, obnlMaterials, limits, hubs, globalSettings) {
					this.parseDate = __bind(this.parseDate, this);
					this.onError = __bind(this.onError, this);
					this.onSaved = __bind(this.onSaved, this);
					this.save = __bind(this.save, this);
					this.removeOBNLReinvestment = __bind(this.removeOBNLReinvestment, this);
					this.removeInvoice = __bind(this.removeInvoice, this);
					this.load = __bind(this.load, this);
					this.isNew = __bind(this.isNew, this);
					this.computeIsMunicipality = __bind(this.computeIsMunicipality, this);
					this.changeOBNLReinvestmentsPage = __bind(this.changeOBNLReinvestmentsPage, this);
					this.changeInvoicesPage = __bind(this.changeInvoicesPage, this); ViewModel.__super__.constructor.call(this, model);
					this.address = new AddressViewModel(model);
					this.invoicesModel = invoices;
					this.obnlReinvestmentsModel = obnlReinvestments;
					this.hub = new HubViewModel(model);
					this.limitsModel = limits;
					this.obnlMaterialsModel = obnlMaterials;
					this.hubs = kb.collectionObservable(hubs.hubs);
					this.invoices = kb.collectionObservable(this.invoicesModel.invoices);
					this.totalInvoices = kb.observable(this.invoicesModel, 'total');
					this.obnlReinvestments = kb.collectionObservable(this.obnlReinvestmentsModel.obnlReinvestments);
					this.totalOBNLReinvestments = kb.observable(this.obnlReinvestmentsModel, 'total');
					this.invoicesPageButtons = kb.collectionObservable(this.invoicesModel.pageButtons);
					this.obnlReinvestmentsPageButtons = kb.collectionObservable(this.obnlReinvestmentsModel.pageButtons);
					this.limits = kb.collectionObservable(this.limitsModel.CurrentLimits);
					this.obnlMaterials = kb.collectionObservable(this.obnlMaterialsModel.CurrentMaterials);
					this.errors = ko.observableArray([]);
					this.globalSettings = globalSettings;
					this.isMunicipality = ko.computed(this.computeIsMunicipality);
				}

				ViewModel.prototype.changeInvoicesPage = function (vm) {
					var page;
					page = vm != null ? vm.number : 1;
					return this.invoicesModel.changePage(page);
				};

				ViewModel.prototype.changeOBNLReinvestmentsPage = function (vm) {
					var page;
					page = vm != null ? vm.number : 1;
					return this.obnlReinvestmentsModel.changePage(page);
				};

				ViewModel.prototype.computeIsMunicipality = function () {
					return this.Category() === null || this.Category() === 'Municipality';
				};

				ViewModel.prototype.isNew = function () {
					return this.model().isNew();
				};

				ViewModel.prototype.load = function (id) {
					this.model().fetch({
						data: {
							id: id
						}
					});
					this.invoicesModel.loadForUser(id);
					this.obnlReinvestmentsModel.loadForUser(id);
					this.limitsModel.fetchByClientId(id);
					return this.obnlMaterialsModel.fetchByClientId(id);
				};

				ViewModel.prototype.removeInvoice = function (inv) {
					if (!confirm(kb.locale_manager.get("Do you want to remove the invoice?"))) {
						return;
					}
					inv.model().destroy();
					return window.location.reload();
				};

				ViewModel.prototype.removeOBNLReinvestment = function (reinv) {
					if (!confirm(kb.locale_manager.get("Do you want to remove the r�emploi?"))) {
						return;
					}
					return reinv.model().destroy();
				};

				ViewModel.prototype.save = function () {
					return this.model().save(null, {
						success: this.onSaved,
						error: this.onError
					});
				};

				ViewModel.prototype.onSaved = function () {
					return window.location.hash = 'client/index';
				};

				ViewModel.prototype.onError = function (m, errors, data) {
					var item, _i, _len, _results;
					errors = $.parseJSON(errors.responseText);
					this.errors.removeAll();
					_results = [];
					for (_i = 0, _len = errors.length; _i < _len; _i++) {
						item = errors[_i];
						_results.push(this.errors.push(item));
					}
					return _results;
				};

				ViewModel.prototype.parseDate = function (item) {
					var createdAt;
					createdAt = item.CreatedAt();
					return moment(createdAt).format('YYYY-MM-DD HH:mm');
				};

				return ViewModel;

			})(kb.ViewModel);

			exports.createViewModel = function () {
				var globalSettings, hubs, invoices, limits, model, obnlMaterials, obnlReinvestments;
				model = new (require('client')).Model();
				invoices = new (require('invoice')).InvoiceList();
				obnlReinvestments = new (require('obnlreinvestment')).OBNLReinvestmentsList();
				obnlMaterials = new (require('obnl.materials')).Model();
				limits = new (require('limits')).Model();
				hubs = new (require('hub')).ListModel();
				hubs.fetch();
				globalSettings = new (require('globalsettings')).Model();
				globalSettings.fetch();
				return new ViewModel(model, invoices, obnlReinvestments, obnlMaterials, limits, hubs, globalSettings);
			};

		}).call(this);
	}, "obnl.materials": function (exports, require, module) {
		(function () {
			var Model, OBNLMaterialRow,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

			OBNLMaterialRow = (function () {

				function OBNLMaterialRow(data) {
					this.Name = data.Name;
					this.Weight = data.Weight;
				}

				return OBNLMaterialRow;

			})();

			Model = (function (_super) {

				__extends(Model, _super);

				Model.prototype.url = 'obnlmaterials';

				function Model() {
					this.fetchByClientId = __bind(this.fetchByClientId, this);
					this.parse = __bind(this.parse, this); this.CurrentMaterials = new Backbone.Collection();
					Model.__super__.constructor.call(this, {});
				}

				Model.prototype.parse = function (resp) {
					var material, _i, _len, _results;
					this.CurrentMaterials.reset();
					_results = [];
					for (_i = 0, _len = resp.length; _i < _len; _i++) {
						material = resp[_i];
						_results.push(this.CurrentMaterials.push(new OBNLMaterialRow(material)));
					}
					return _results;
				};

				Model.prototype.fetchByClientId = function (id) {
					return this.fetch({
						data: {
							clientId: id
						}
					});
				};

				return Model;

			})(Backbone.Model);

			exports.Model = Model;

		}).call(this);
	}, "common": function (exports, require, module) {
		(function () {
			var LocalizedStringLocalizer, generatePages;

			LocalizedStringLocalizer = kb.LocalizedObservable.extend({
				constructor: function (value, options, view_model) {
					kb.LocalizedObservable.prototype.constructor.apply(this, arguments);
					return kb.utils.wrappedObservable(this);
				},
				read: function (string_id) {
					return kb.locale_manager.get(string_id);
				}
			});

			generatePages = function (current, count) {
				var fp, i, lp, page, result;
				fp = current - 5;
				if (fp < 1) fp = 1;
				lp = current + 5;
				if (lp > count) lp = count;
				result = [];
				if (fp > 1) {
					result.push({
						number: 1,
						isActive: false
					});
				}
				for (i = fp; fp <= lp ? i <= lp : i >= lp; fp <= lp ? i++ : i--) {
					page = {
						number: i,
						isActive: i === current
					};
					result.push(page);
				}
				if (lp < count) {
					result.push({
						number: count,
						isActive: false
					});
				}
				return result;
			};

			exports.LocalizedStringLocalizer = LocalizedStringLocalizer;

			exports.generatePages = generatePages;

		}).call(this);
	}, "dashboard": function (exports, require, module) {
		(function () {
			var Model, ViewModel,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			Model = (function (_super) {

				__extends(Model, _super);

				Model.prototype.url = 'dashboard';

				function Model() {
					this.parse = __bind(this.parse, this);
					this.fetch = __bind(this.fetch, this); Model.__super__.constructor.call(this, {
						InvoicesToday: '',
						InvoicesThisMonth: '',
						InvoicesThisYear: '',
						ClientsToday: '',
						ClientsThisMonth: '',
						ClientsThisYear: '',
						OBNLVisitsToday: '',
						OBNLVisitsThisMonth: '',
						OBNLVisitsThisYear: '',
						WeightToday: '',
						WeightThisMonth: '',
						WeightThisYear: '',
						MaxExceeded: '',
						MonthLog: void 0,
						EcoCentersSummary: [],
						EcoCentersTotal: {
							Clients: 0,
							Visits: 0,
							OBNLVisits: 0,
							OBNLWeight: 0
						},
						From: Date.today().moveToFirstDayOfMonth(),
						To: Date.today().moveToLastDayOfMonth()
					});
				}

				Model.prototype.fetch = function () {
					var args;
					args = {
						data: {
							From: this.attributes.From.toString('yyyy-MM-dd'),
							To: this.attributes.To.toString('yyyy-MM-dd')
						}
					};
					return Model.__super__.fetch.call(this, args);
				};

				Model.prototype.parse = function (resp) {
					resp.EcoCentersTotal = {
						Visits: resp.EcoCentersSummary[0].Visits,
						Clients: resp.EcoCentersSummary[0].Clients,
						OBNLVisits: resp.EcoCentersSummary[0].OBNLVisits,
						OBNLWeight: resp.EcoCentersSummary[0].OBNLWeight
					};
					resp.EcoCentersSummary.splice(0, 1);
					return Model.__super__.parse.call(this, resp);
				};

				return Model;

			})(Backbone.Model);

			ViewModel = (function (_super) {

				__extends(ViewModel, _super);

				function ViewModel() {
					this.load = __bind(this.load, this); ViewModel.__super__.constructor.call(this, new Model());
				}

				ViewModel.prototype.load = function () {
					return this.model().fetch();
				};

				return ViewModel;

			})(kb.ViewModel);

			exports.createViewModel = function () {
				return new ViewModel();
			};

		}).call(this);
	}, "empty.module": function (exports, require, module) {
		(function () {
			var _this = this;

			exports.createViewModel = function () {
				return {};
			};

		}).call(this);
	}, "giveaway": function (exports, require, module) {
		(function () {
			var GiveawayListModel, GiveawayModel,
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; };

			GiveawayModel = (function (_super) {

				__extends(GiveawayModel, _super);

				GiveawayModel.prototype.url = 'giveaway/index';

				GiveawayModel.prototype.defaults = {
					Id: null,
					Title: '',
					Description: '',
					Price: 0
				};

				function GiveawayModel(args) {
					this.url = 'giveaway/index';
					this.idAttribute = 'Id';
					GiveawayModel.__super__.constructor.call(this, args);
				}

				return GiveawayModel;

			})(Backbone.Model);

			GiveawayListModel = (function (_super) {

				__extends(GiveawayListModel, _super);

				function GiveawayListModel(models, options) {
					this.search = __bind(this.search, this); this.url = 'giveaway/list';
					this.model = GiveawayModel;
					GiveawayListModel.__super__.constructor.call(this, models, options);
				}

				GiveawayListModel.prototype.search = function (term) {
					return this.fetch({
						data: $.param({
							q: term,
							isGivenAway: false,
							onlyCurrentHub: true
						})
					});
				};

				return GiveawayListModel;

			})(Backbone.Collection);

			exports.Model = GiveawayModel;

			exports.ListModel = GiveawayListModel;

		}).call(this);
	}, "globalsettings": function (exports, require, module) {
		(function () {
			var GlobalSettingsModel,
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

			GlobalSettingsModel = (function (_super) {

				__extends(GlobalSettingsModel, _super);

				function GlobalSettingsModel() {
					GlobalSettingsModel.__super__.constructor.call(this, {
						MaxYearlyClientVisits: 0,
						MaxYearlyClientVisitsWarning: 0,
						QstTaxRate: 0,
						GstTaxRate: 0,
						GstTaxLine: "",
						QstTaxLine: "",
						DefaultMaterialUnit: "",
						SessionTimeoutInMinutes: ""
					});
					this.url = 'globalsettings';
				}

				return GlobalSettingsModel;

			})(Backbone.Model);

			exports.Model = GlobalSettingsModel;

		}).call(this);
	}, "globalsettings.form": function (exports, require, module) {
		(function () {
			var ViewModel,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

			ViewModel = (function (_super) {

				__extends(ViewModel, _super);

				function ViewModel(model) {
					this.onError = __bind(this.onError, this);
					this.onSaved = __bind(this.onSaved, this);
					this.save = __bind(this.save, this);
					this.load = __bind(this.load, this); ViewModel.__super__.constructor.call(this, model);
					this.maxYearlyClientVisits = kb.observable(model, 'MaxYearlyClientVisits');
					this.maxYearlyClientVisitsWarning = kb.observable(model, 'MaxYearlyClientVisitsWarning');
					this.adminNotificationsEmail = kb.observable(model, 'AdminNotificationsEmail');
					this.containerFullNotificationsEmail = kb.observable(model, 'ContainerFullNotificationsEmail');
					this.gstTaxRate = kb.observable(model, 'GstTaxRate');
					this.qstTaxRate = kb.observable(model, 'QstTaxRate');
					this.gstTaxLine = kb.observable(model, 'GstTaxLine');
					this.qstTaxLine = kb.observable(model, 'QstTaxLine');
					this.defaultMaterialUnit = kb.observable(model, 'DefaultMaterialUnit');
					this.sessionTimeoutInMinutes = kb.observable(model, 'SessionTimeoutInMinutes');
					this.errors = ko.observableArray([]);
				}

				ViewModel.prototype.load = function () {
					return this.model().fetch();
				};

				ViewModel.prototype.save = function () {
					return this.model().save(null, {
						success: this.onSaved,
						error: this.onError
					});
				};

				ViewModel.prototype.onSaved = function () {
					this.errors.removeAll();
					eco.app.notifications.addNotification('msg-global-settings-saved-successfully');
				};

				ViewModel.prototype.onError = function (m, errors, data) {
					var item, _i, _len, _results;
					errors = $.parseJSON(errors.responseText);
					this.errors.removeAll();
					eco.app.notifications.removeNotification('msg-global-settings-saved-successfully');
					_results = [];
					for (_i = 0, _len = errors.length; _i < _len; _i++) {
						item = errors[_i];
						_results.push(this.errors.push(item));
					}
					return _results;
				};

				return ViewModel;

			})(Knockback.ViewModel);

			exports.createViewModel = function () {
				var model;
				model = new (require('globalsettings')).Model();
				return new ViewModel(model);
			};

		}).call(this);
	}, "hub": function (exports, require, module) {
		(function () {
			var HubListModel, HubModel,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

			HubModel = (function (_super) {

				__extends(HubModel, _super);

				HubModel.prototype.url = 'hub';

				HubModel.prototype.defaults = {
					Id: null,
					Name: '',
					InvoiceIdentifier: '',
					Address: '',
					DefaultGiveawayPrice: 0
				};

				function HubModel(args) {
					this.fetchCurrent = __bind(this.fetchCurrent, this); this.url = 'hub';
					this.idAttribute = 'Id';
					HubModel.__super__.constructor.call(this, args);
				}

				HubModel.prototype.fetchCurrent = function () {
					return this.fetch({
						url: "hub/current",
						async: false
					});
				};

				return HubModel;

			})(Backbone.Model);

			HubListModel = (function (_super) {

				__extends(HubListModel, _super);

				function HubListModel() {
					this.parse = __bind(this.parse, this);
					HubListModel.__super__.constructor.apply(this, arguments);
				}

				HubListModel.prototype.url = 'hub';

				HubListModel.prototype.hubs = new Backbone.Collection();

				HubListModel.prototype.parse = function (resp) {
					var item, row, _i, _len, _results;
					this.hubs.reset();
					this.hubList = this.hubs;
					_results = [];
					for (_i = 0, _len = resp.length; _i < _len; _i++) {
						item = resp[_i];
						row = new HubModel(item);
						_results.push(this.hubs.push(row));
					}
					return _results;
				};

				return HubListModel;

			})(Backbone.Model);

			exports.Model = HubModel;

			exports.ListModel = HubListModel;

		}).call(this);
	}, "hub.form": function (exports, require, module) {
		(function () {
			var ViewModel,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				_this = this;

			ViewModel = (function () {

				function ViewModel(model) {
					this.save = __bind(this.save, this);
					this.load = __bind(this.load, this);
					this.updateNew = __bind(this.updateNew, this); this.model = model;
					this.id = kb.observable(model, 'Id');
					this.name = kb.observable(model, 'Name');
					this.invoiceIdentifier = kb.observable(model, 'InvoiceIdentifier');
					this.defaultGiveawayPrice = kb.observable(model, 'DefaultGiveawayPrice');
					this.address = kb.observable(model, 'Address');
					this.isNew = ko.observable(true);
					model.on('change', this.updateNew);
				}

				ViewModel.prototype.updateNew = function () {
					return this.isNew(this.model.isNew());
				};

				ViewModel.prototype.load = function (id) {
					return this.model.fetch({
						data: {
							id: id
						}
					});
				};

				ViewModel.prototype.save = function () {
					this.model.save();
					return window.location.hash = 'hub/index';
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var model, module;
				module = require('hub');
				model = new module.Model();
				return new ViewModel(model);
			};

		}).call(this);
	}, "hub.list": function (exports, require, module) {
		(function () {
			var ViewModel,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				_this = this;

			ViewModel = (function () {

				function ViewModel(model) {
					this.load = __bind(this.load, this); this.model = model;
					this.hubs = kb.collectionObservable(this.model.hubs);
				}

				ViewModel.prototype.load = function () {
					return this.model.fetch();
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var model;
				model = new (require('hub')).ListModel();
				return new ViewModel(model);
			};

		}).call(this);
	}, "invoice": function (exports, require, module) {
		(function () {
			var InvoiceList, InvoiceModel, NewInvoiceModel, PickedMaterial, common,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

			common = require('common');

			InvoiceList = (function (_super) {

				__extends(InvoiceList, _super);

				InvoiceList.prototype.invoices = new Backbone.Collection();

				InvoiceList.prototype.pageButtons = new Backbone.Collection();

				InvoiceList.prototype.urlRoot = '/invoice/index';

				function InvoiceList() {
					this.changePage = __bind(this.changePage, this);
					this.search = __bind(this.search, this);
					this.parse = __bind(this.parse, this);
					this.loadForUser = __bind(this.loadForUser, this); this.invoices.url = '/invoice/index';
					InvoiceList.__super__.constructor.call(this, {
						filterTerm: '',
						filterTermFrom: null,
						filterTermTo: null,
						filterType: 'invoiceNo',
						filterDeleted: false,
						CurrentYear: true,
						sortBy: 'invoiceDate',
						sortDir: 'desc',
						userId: '',
						page: 1,
						pageCount: 1,
						pageSize: null,
						centerName: 'Tous',
						total: 0
					});
				}

				InvoiceList.prototype.loadForUser = function (id) {
					this.set('userId', id);
					return this.search();
				};

				InvoiceList.prototype.parse = function (resp) {
					var item, model, page, pages, _i, _j, _len, _len2, _ref, _results;
					this.invoices.reset();
					this.set('total', resp.Total);
					_ref = resp.Invoices;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						model = new InvoiceModel;
						model.set(item);
						this.invoices.push(model);
					}
					this.pageButtons.reset();
					pages = common.generatePages(resp.Page, resp.PageCount);
					this.set('page', resp.Page);
					this.set('pageCount', resp.PageCount);
					_results = [];
					for (_j = 0, _len2 = pages.length; _j < _len2; _j++) {
						page = pages[_j];
						_results.push(this.pageButtons.push(page));
					}
					return _results;
				};

				InvoiceList.prototype.search = function () {
					return this.changePage(1);
				};

				InvoiceList.prototype.changePage = function (page) {
					var $loadingFade, fetchAjax, from, to,
						_this = this;
					this.set('page', page);
					from = this.get('filterTermFrom');
					to = this.get('filterTermTo');
					if (from != null) from = from.toString('yyyy-MM-dd');
					if (to != null) to = to.toString('yyyy-MM-dd');
					$loadingFade = $("#global-loading-fade");
					$loadingFade.modal('show');
					fetchAjax = this.fetch({
						data: {
							page: this.get('page'),
							pageSize: this.get('pageSize'),
							userId: this.get('userId'),
							sortDir: this.get('sortDir'),
							sortBy: this.get('sortBy'),
							CurrentYear: this.get('CurrentYear'),
							Deleted: this.get('filterDeleted'),
							Type: this.get('filterType'),
							Term: this.get('filterTerm'),
							TermFrom: from,
							TermTo: to,
							CenterName: this.get('centerName')
						}
					});
					return fetchAjax.complete(function () {
						var setTimeoutCallback;
						setTimeoutCallback = function () {
							return $loadingFade.modal('hide');
						};
						setTimeout(setTimeoutCallback, 500);
						return fetchAjax;
					});
				};

				return InvoiceList;

			})(Backbone.Model);

			InvoiceModel = (function (_super) {

				__extends(InvoiceModel, _super);

				InvoiceModel.prototype.urlRoot = '/invoice/index';

				InvoiceModel.prototype.idAttribute = 'Id';

				function InvoiceModel() {
					this.url = __bind(this.url, this);
					this.fetchById = __bind(this.fetchById, this); InvoiceModel.__super__.constructor.call(this, {
						InvoiceNo: '',
						Id: '',
						CreatedAt: '',
						Comment: '',
						CreatedBy: null,
						Attachments: [],
						Materials: [],
						GiveawayItems: [],
						IsOBNL: false,
						AmountIncludingTaxes: '',
						Amount: '',
						Address: new Backbone.Model({
							City: 'c',
							CItyId: 'i',
							Street: 'd',
							CivicNumber: 's',
							AptNumber: '',
							PostalCode: 'a'
						}),
						Client: {
							FirstName: '',
							LastName: '',
							PhoneNumber: '',
							Address: new Backbone.Model({
								City: '',
								CItyId: '',
								Street: '',
								CivicNumber: '',
								AptNumber: '',
								PostalCode: ''
							})
						},
						Center: {
							Name: '',
							Url: ''
						},
						Taxes: [],
						VisitNumber: 0,
						VisitLimit: null
					});
				}

				InvoiceModel.prototype.fetchById = function (id) {
					return this.fetch({
						data: {
							id: id
						}
					});
				};

				InvoiceModel.prototype.url = function () {
					if (this.isNew()) return "/invoice";
					return "/invoice/index/" + this.id;
				};

				return InvoiceModel;

			})(Backbone.Model);

			PickedMaterial = (function (_super) {

				__extends(PickedMaterial, _super);

				function PickedMaterial() {
					PickedMaterial.__super__.constructor.call(this, {
						Id: '',
						Quantity: 0,
						Container: '',
						ProvidedProofOfResidence: false
					});
				}

				return PickedMaterial;

			})(Backbone.Model);

			NewInvoiceModel = (function (_super) {

				__extends(NewInvoiceModel, _super);

				NewInvoiceModel.prototype.url = 'invoice';

				function NewInvoiceModel() {
					this.fetchPreview = __bind(this.fetchPreview, this);
					this.removeMaterial = __bind(this.removeMaterial, this);
					this.clearMaterials = __bind(this.clearMaterials, this);
					this.addMaterial = __bind(this.addMaterial, this);
					this.selectClient = __bind(this.selectClient, this); this.Client = ko.observable(null);
					NewInvoiceModel.__super__.constructor.call(this, {
						Id: '',
						Comment: '',
						CreatedBy: null,
						ClientId: 0,
						EmployeeName: 0,
						Materials: [],
						Attachments: [],
						GiveawayItems: []
					});
				}

				NewInvoiceModel.prototype.selectClient = function (client) {
					this.Client(client);
					return this.set({
						ClientId: client.get('Id')
					});
				};

				NewInvoiceModel.prototype.addMaterial = function (material) {
					return this.get('Materials').push(material);
				};

				NewInvoiceModel.prototype.clearMaterials = function () {
					return this.set({
						'Materials': []
					});
				};

				NewInvoiceModel.prototype.removeMaterial = function (material) {
					var newArray,
						_this = this;
					newArray = this.get('Materials');
					newArray = newArray.length <= 1 ? [] : $.grep(newArray, function (v) {
						return v.get('Id') !== material.get('Id');
					});
					return this.set({
						'Materials': newArray
					});
				};

				NewInvoiceModel.prototype.fetchPreview = function (success, error) {
					var model, options;
					options = {};
					options.url = '/invoice/preview';
					options.success = success;
					options.error = error;
					model = new InvoiceModel();
					return Backbone.sync.apply(model, ['create', this, options]);
				};

				return NewInvoiceModel;

			})(Backbone.Model);
			NewInvoiceModel1 = (function (_super) {

				__extends(NewInvoiceModel1, _super);

				NewInvoiceModel1.prototype.url = '/invoice/new1';

				function NewInvoiceModel1() {
					this.fetchPreview = __bind(this.fetchPreview, this);
					this.removeMaterial = __bind(this.removeMaterial, this);
					this.clearMaterials = __bind(this.clearMaterials, this);
					this.addMaterial = __bind(this.addMaterial, this);
					this.selectClient = __bind(this.selectClient, this); this.Client = ko.observable(null);
					NewInvoiceModel1.__super__.constructor.call(this, {
						Id: '',
						Comment: '',
						CreatedBy: null,
						ClientId: 0,
						EmployeeName: 0,
						Materials: [],
						Attachments: [],
						GiveawayItems: []
					});
				}

				NewInvoiceModel1.prototype.selectClient = function (client) {
					this.Client(client);
					return this.set({
						ClientId: client.get('Id')
					});
				};

				NewInvoiceModel1.prototype.addMaterial = function (material) {
					return this.get('Materials').push(material);
				};

				NewInvoiceModel1.prototype.clearMaterials = function () {
					return this.set({
						'Materials': []
					});
				};

				NewInvoiceModel1.prototype.removeMaterial = function (material) {
					var newArray,
						_this = this;
					newArray = this.get('Materials');
					newArray = newArray.length <= 1 ? [] : $.grep(newArray, function (v) {
						return v.get('Id') !== material.get('Id');
					});
					return this.set({
						'Materials': newArray
					});
				};

				NewInvoiceModel1.prototype.fetchPreview = function (success, error) {
					var model, options;
					options = {};
					options.url = '/invoice/preview';
					options.success = success;
					options.error = error;
					model = new InvoiceModel();
					return Backbone.sync.apply(model, ['create', this, options]);
				};

				return NewInvoiceModel1;

			})(Backbone.Model);

			exports.InvoiceList = InvoiceList;

			exports.InvoiceModel = InvoiceModel;

			exports.NewInvoiceModel = NewInvoiceModel;

			exports.NewInvoiceModel1 = NewInvoiceModel1;

			exports.PickedMaterial = PickedMaterial;

		}).call(this);
	}, "invoice.form": function (exports, require, module) {
		(function () {
			var ClientViewModel, GiveawayViewModel, HubModel, MaterialViewModel, MaterialsViewModel, PickedMaterial, PickedMaterialViewModel, ViewModel, client, giveaway, invoice,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

			client = require('client');

			invoice = require('invoice');

			giveaway = require('giveaway');

			PickedMaterial = require('invoice').PickedMaterial;

			HubModel = require('hub').Model;

			ClientViewModel = (function (_super) {

				__extends(ClientViewModel, _super);

				function ClientViewModel(model) {
					this.showFirstNameLastName = __bind(this.showFirstNameLastName, this);
					this.computeShowContactWarning = __bind(this.computeShowContactWarning, this);
					this.computeShowComment = __bind(this.computeShowComment, this);
					this.computeShowUnverifiedWarning = __bind(this.computeShowUnverifiedWarning, this);
					this.computeFullName = __bind(this.computeFullName, this);
					this.computeShowMaxVisitsReachedWarning = __bind(this.computeShowMaxVisitsReachedWarning, this);
					this.computeShowMaxVisitsWarning = __bind(this.computeShowMaxVisitsWarning, this);
					this.showIsRegisteredInAnotherHubAlert = __bind(this.showIsRegisteredInAnotherHubAlert, this);
					this.showMaxVisitsWarningAlert = __bind(this.showMaxVisitsWarningAlert, this);
					this.showMaxVisitsReachedAlert = __bind(this.showMaxVisitsReachedAlert, this);
					this.filterIncludedInvoices = __bind(this.filterIncludedInvoices, this);
					this.filterExcludedInvoices = __bind(this.filterExcludedInvoices, this); ClientViewModel.__super__.constructor.call(this, model);
					this.model = model;
					this.categories = new (require('client')).CategoriesModel();
					this.categories.fetch();
					this.fullName = ko.computed(this.computeFullName);
					this.limitsModel = new (require('limits')).Model();
					this.limitsModel.fetchByClientId(this.model.get('Id'));
					this.limits = kb.collectionObservable(this.limitsModel.CurrentLimits);
					this.obnlMaterialsModel = new (require('obnl.materials')).Model();
					this.obnlMaterialsModel.fetchByClientId(this.model.get('Id'));
					this.obnlMaterials = kb.collectionObservable(this.obnlMaterialsModel.CurrentMaterials);
					this.globalSettingsModel = new (require('globalsettings')).Model();
					this.globalSettingsModel.fetch({
						async: false
					});
					this.maxYearlyClientVisitsWarning = ko.observable(this.globalSettingsModel.get('MaxYearlyClientVisitsWarning'));
					this.maxYearlyClientVisits = ko.observable(this.globalSettingsModel.get('MaxYearlyClientVisits'));
					this.invoices = kb.collectionObservable(this.model.Invoices);
					this.excludedInvoices = ko.computed(this.filterExcludedInvoices);
					this.includedInvoices = ko.computed(this.filterIncludedInvoices);
					this.showContactWarning = ko.computed(this.computeShowContactWarning);
					this.showUnverifiedWarning = ko.computed(this.computeShowUnverifiedWarning);
					this.showComment = ko.computed(this.computeShowComment);
					this.showMaxVisitsWarning = ko.computed(this.computeShowMaxVisitsWarning);
					this.showMaxVisitsReachedWarning = ko.computed(this.computeShowMaxVisitsReachedWarning);
					this.isRegisteredInCurrentHub = ko.observable(this.model.get('IsRegisteredInCurrentHub'));
					this.showIsRegisteredInAnotherHubWarning = ko.observable(!this.model.get('IsRegisteredInCurrentHub'));
				}

				ClientViewModel.prototype.filterExcludedInvoices = function () {
					var _this = this;
					return ko.utils.arrayFilter(this.invoices(), function (item) {
						return item.IsExcluded();
					});
				};

				ClientViewModel.prototype.filterIncludedInvoices = function () {
					var _this = this;
					return ko.utils.arrayFilter(this.invoices(), function (item) {
						return !item.IsExcluded();
					});
				};

				ClientViewModel.prototype.showMaxVisitsReachedAlert = function () {
					return $('#max-visits-reached-msg-modal').modal('show');
				};

				ClientViewModel.prototype.showMaxVisitsWarningAlert = function () {
					return $('#max-visits-warning-msg-modal').modal('show');
				};

				ClientViewModel.prototype.showIsRegisteredInAnotherHubAlert = function () {
					return $('#is-registered-in-another-hub-msg-modal').modal('show');
				};

				ClientViewModel.prototype.computeShowMaxVisitsWarning = function () {
					if (this.PersonalVisitsLimit() && this.PersonalVisitsLimit() > 0) {
						return this.invoices().length >= (this.PersonalVisitsLimit() - this.maxYearlyClientVisitsWarning() - 1) && this.maxYearlyClientVisitsWarning() > 0 && this.invoices().length < this.PersonalVisitsLimit();
					}
					return this.invoices().length >= (this.maxYearlyClientVisits() - this.maxYearlyClientVisitsWarning() - 1) && this.maxYearlyClientVisitsWarning() > 0 && this.invoices().length < this.maxYearlyClientVisits();
				};

				ClientViewModel.prototype.computeShowMaxVisitsReachedWarning = function () {
					if (this.PersonalVisitsLimit() && this.PersonalVisitsLimit() > 0) {
						return this.invoices().length >= this.PersonalVisitsLimit();
					}
					return this.invoices().length >= this.maxYearlyClientVisits() && this.maxYearlyClientVisits() > 0;
				};

				ClientViewModel.prototype.computeFullName = function () {
					return this.FirstName() + ' ' + this.LastName();
				};

				ClientViewModel.prototype.computeShowUnverifiedWarning = function () {
					return !this.Verified();
				};

				ClientViewModel.prototype.computeShowComment = function () {
					return this.Comments() !== null && this.Comments() !== '';
				};

				ClientViewModel.prototype.computeShowContactWarning = function () {
					var email, phone;
					email = this.Email();
					phone = this.PhoneNumber();
					return email === null || email.length < 1 || phone === null || phone.length < 0;
				};

				ClientViewModel.prototype.showFirstNameLastName = function () {
					return this.categories.showFirstNameLastName(this.get('Category'));
				};

				return ClientViewModel;

			})(kb.ViewModel);

			PickedMaterialViewModel = (function (_super) {

				__extends(PickedMaterialViewModel, _super);

				function PickedMaterialViewModel(model, material, hub) {
					this.loadContainers = __bind(this.loadContainers, this);
					var currentHubId, hubSettings, viewModel,
						_this = this;
					PickedMaterialViewModel.__super__.constructor.call(this, model);
					this.model = model;
					this.width = ko.observable('');
					this.height = ko.observable('');
					this.depth = ko.observable('');
					this.containers = ko.observableArray([]);
					this.selectedContainer = ko.observable;
					viewModel = this;
					this.enteredQuantity = '';
					this.hasFocus = ko.observable(true);
					this.name = material.get("Name");
					this.unit = material.get("Unit");
					this.needsCalculator = this.unit === 'v\u00B3' || this.unit === 'v3';
					currentHubId = hub.get("Id");
					hubSettings = _.find(material.get("HubSettings"), function (x) {
						return x.HubId === currentHubId;
					}) || {};
					this.needsProofOfResidence = hubSettings.RequireProofOfResidence;
					this.hasContainer = hubSettings.HasContainer;
					if (this.hasContainer) this.loadContainers();
					this.Quantity = kb.observable(model, {
						key: 'Quantity',
						read: function () {
							var d, h, quantity, result, toDecimal, w;
							toDecimal = function (i) {
								return (i || "").replace(',', '.').replace(/[^0-9\.]/g, '');
							};
							w = toDecimal(viewModel.width()) || 0;
							h = toDecimal(viewModel.height()) || 0;
							d = toDecimal(viewModel.depth()) || 0;
							quantity = new BigNumber(w).mul(h).mul(d).div(3 * 3 * 3).round(3);
							if (quantity > 0) {
								result = quantity.toString().replace('.', ',');
								model.set('Quantity', result);
								return result;
							} else {
								model.set('Quantity', viewModel.enteredQuantity);
								return viewModel.enteredQuantity;
							}
						},
						write: function (value) {
							viewModel.enteredQuantity = value;
							viewModel.width('');
							viewModel.height('');
							return viewModel.depth('');
						}
					}, {}, this);
				}

				PickedMaterialViewModel.prototype.loadContainers = function () {
					var id,
						_this = this;
					id = this.model.get('Id');
					return $.getJSON("/container/list?onlyCurrentHub=true&materialId=" + id).done(function (result) {
						return _this.containers(result.Items || []);
					});
				};

				return PickedMaterialViewModel;

			})(kb.ViewModel);

			MaterialViewModel = (function () {

				function MaterialViewModel(material) {
					this.model = material;
					this.Name = material.get('Name');
					this.Unit = material.get('Unit');
					this.Weight = '';
					this.IsExcluded = material.IsExcluded;
					this.Id = material.Id;
				}

				return MaterialViewModel;

			})();

			MaterialsViewModel = (function () {

				function MaterialsViewModel(model, hubModel) {
					this.sortMaterials = __bind(this.sortMaterials, this);
					this.sortCallback = __bind(this.sortCallback, this);
					this.removeMaterial = __bind(this.removeMaterial, this);
					this.pickMaterial = __bind(this.pickMaterial, this);
					this.showAvailableList = __bind(this.showAvailableList, this);
					this.updateAvailable = __bind(this.updateAvailable, this);
					this.reload = __bind(this.reload, this);
					var _this = this;
					this.model = model;
					this.materialsModel = new (require('material')).ListModel();
					this.materialsModel.filter = function (m) {
						return m.Active;
					};
					this.materialsModel.materials.bind('add', this.updateAvailable);
					this.availableMaterials = ko.observableArray([]);
					this.pickedMaterials = ko.observableArray([]);
					this.showAvailable = ko.observable(false);
					this.removedMaterials = {};
					this.hub = hubModel;
				}

				MaterialsViewModel.prototype.reload = function (client) {
					var cityId, options;
					this.availableMaterials([]);
					this.pickedMaterials([]);
					this.removedMaterials = {};
					cityId = client.get("Address").CityId;
					options = {
						onlyCurrentHub: true,
						municipality: cityId
					};
					return this.materialsModel.fetch(options);
				};

				MaterialsViewModel.prototype.updateAvailable = function (item) {
					return this.availableMaterials.push(new MaterialViewModel(item));
				};

				MaterialsViewModel.prototype.showAvailableList = function () {
					return this.showAvailable(true);
				};

				MaterialsViewModel.prototype.pickMaterial = function (m) {
					var pickedMaterial, pickedMaterialVm;
					this.availableMaterials.remove(m);
					this.removedMaterials[m.model.get("Id")] = m;
					pickedMaterial = new PickedMaterial();
					pickedMaterial.set("Id", m.model.get("Id"));
					this.model.addMaterial(pickedMaterial);
					pickedMaterialVm = new PickedMaterialViewModel(pickedMaterial, m.model, this.hub);
					this.pickedMaterials.push(pickedMaterialVm);
					return this.sortMaterials();
				};

				MaterialsViewModel.prototype.removeMaterial = function (m) {
					var id, originalMaterial;
					this.model.removeMaterial(m.model);
					id = m.model.get("Id");
					originalMaterial = this.removedMaterials[id];
					this.availableMaterials.push(originalMaterial);
					this.pickedMaterials.remove(m);
					return this.sortMaterials();
				};

				MaterialsViewModel.prototype.sortCallback = function (l, r) {
					if (l.Name === r.Name) return 0;
					if (l.Name < r.Name) {
						return -1;
					} else {
						return 1;
					}
				};

				MaterialsViewModel.prototype.sortMaterials = function () {
					this.availableMaterials.sort(this.sortCallback);
					return this.pickedMaterials.sort(this.sortCallback);
				};

				return MaterialsViewModel;

			})();

			GiveawayViewModel = (function () {

				function GiveawayViewModel() {
					this.search = __bind(this.search, this);
					this.getImageUrl = __bind(this.getImageUrl, this);
					this.removeItem = __bind(this.removeItem, this);
					this.addItem = __bind(this.addItem, this);
					this.hideChosenItems = __bind(this.hideChosenItems, this);
					this.computeShowChosenItems = __bind(this.computeShowChosenItems, this); this.model = new giveaway.ListModel();
					this.searchTerm = ko.observable("");
					this.selectedItems = ko.observableArray();
					this.showSelectedItems = ko.computed(this.computeShowChosenItems);
					this.items = kb.collectionObservable(this.model);
					this.model.search();
				}

				GiveawayViewModel.prototype.computeShowChosenItems = function () {
					return this.selectedItems().length;
				};

				GiveawayViewModel.prototype.hideChosenItems = function () {
					return this.showChosenItems(false);
				};

				GiveawayViewModel.prototype.addItem = function (item) {
					var _this = this;
					if (_(this.selectedItems()).find(function (x) {
						return x.Id() === item.Id();
					})) {
						return;
					}
					this.selectedItems.push(item);
					return this.items.remove(item);
				};

				GiveawayViewModel.prototype.removeItem = function (item) {
					this.selectedItems.remove(item);
					return this.items.push(item);
				};

				GiveawayViewModel.prototype.getImageUrl = function (item) {
					return '/giveaway/image/' + item.ImageId();
				};

				GiveawayViewModel.prototype.search = function () {
					return this.model.search(this.searchTerm());
				};

				return GiveawayViewModel;

			})();

			ViewModel = (function () {

				function ViewModel(model, clientListViewModel, showClients, hubModel) {
					this.onError = __bind(this.onError, this);
					this.newClient = __bind(this.newClient, this);
					this.scanCitizenCard = __bind(this.scanCitizenCard, this);
					this.editClient = __bind(this.editClient, this);
					this.changeClient = __bind(this.changeClient, this);
					this.onSaved = __bind(this.onSaved, this);
					this.save = __bind(this.save, this);
					this.navigateToPreview = __bind(this.navigateToPreview, this);
					this.hidePreview = __bind(this.hidePreview, this);
					this.getGiveawayItems = __bind(this.getGiveawayItems, this);
					this.preview = __bind(this.preview, this);
					this.onPreview = __bind(this.onPreview, this);
					this.computePreviewAvailable = __bind(this.computePreviewAvailable, this);
					this.fileUploaded = __bind(this.fileUploaded, this);
					this.onClientSelected = __bind(this.onClientSelected, this); this.model = model;
					this.clientList = clientListViewModel;
					this.errors = ko.observableArray();
					this.client = ko.observable(null);
					this.invoicePreview = ko.observable(null);
					model.Client.subscribe(this.onClientSelected);
					this.clientId = kb.observable(model, 'ClientId');
					this.employeeName = kb.observable(model, 'EmployeeName');
					this.comment = kb.observable(model, 'Comment');
					this.showList = ko.observable(true);
					this.showListPoc = ko.observable(true);
					this.saved = false;
					this.attachments = ko.observableArray([]);
					this.previewAvailable = ko.computed(this.computePreviewAvailable);
					this.fileUpload = {
						dataType: 'json',
						done: this.fileUploaded
					};
					if (showClients) this.clientList.search();
					this.giveaway = new GiveawayViewModel(model);
					this.materials = new MaterialsViewModel(this.model, hubModel);
				}

				ViewModel.prototype.onClientSelected = function (client) {
					this.client(new ClientViewModel(client));
					this.materials.reload(client);
					return this.model.clearMaterials();
				};

				ViewModel.prototype.fileUploaded = function (e, f) {
					var att, resp, result;
					resp = f.jqXHR.responseText;
					if (resp === void 0) resp = f.jqXHR.iframe[0].documentElement.innerText;
					result = $.parseJSON(resp);
					att = this.model.get('Attachments');
					att.push(result);
					return this.attachments.push(result);
				};

				ViewModel.prototype.computePreviewAvailable = function () {
					return this.client() !== null;
				};

				ViewModel.prototype.onPreview = function (response) {
					response.CreatedAtFormatted = ko.observable('');
					response.IsOBNL = ko.observable(response.IsOBNL);
					this.invoicePreview(response);
					return this.navigateToPreview;
				};

				ViewModel.prototype.preview = function () {
					this.errors.removeAll();
					this.model.set('GiveawayItems', this.getGiveawayItems());
					return this.model.fetchPreview(this.onPreview, this.onError);
				};

				ViewModel.prototype.getGiveawayItems = function () {
					var _this = this;
					return _.map(this.giveaway.selectedItems(), function (item) {
						return {
							id: item.Id()
						};
					});
				};

				ViewModel.prototype.hidePreview = function () {
					return this.invoicePreview(null);
				};

				ViewModel.prototype.navigateToPreview = function () {
					return eco.app.router.navigate('/invoice/new/preview');
				};

				ViewModel.prototype.save = function () {
					$("#global-loading-fade").modal('show');
					this.model.set('GiveawayItems', this.getGiveawayItems());
					return this.model.save(null, {
						success: this.onSaved,
						error: this.onError
					});
				};

				ViewModel.prototype.onSaved = function () {
					var setTimeoutCallback;
					setTimeoutCallback = function () {
						return $("#global-loading-fade").modal('hide');
					};
					setTimeout(setTimeoutCallback, 500);
					this.saved = true;
					eco.app.notifications.addNotification('msg-invoice-saved-successfully');
					return eco.app.router.navigate('invoice/show/' + this.model.get('Id'), {
						trigger: true
					});
				};

				ViewModel.prototype.changeClient = function () {
					this.showListPoc(true);
					return this.showList(true);
				};

				ViewModel.prototype.editClient = function () {
					return eco.app.router.navigate('invoice/new/client/edit/' + this.client().Id(), {
						trigger: true
					});
				};

				ViewModel.prototype.newClient = function () {
					return eco.app.router.navigate('invoice/new/client/create', {
						trigger: true
					});
				};

				ViewModel.prototype.scanCitizenCard = function () {

				};

				ViewModel.prototype.onError = function (m, errors, data) {
					var item, setTimeoutCallback, _i, _len, _results;
					setTimeoutCallback = function () {
						return $("#global-loading-fade").modal('hide');
					};
					setTimeout(setTimeoutCallback, 500);
					errors = $.parseJSON(errors.responseText);
					this.errors.removeAll();
					_results = [];
					for (_i = 0, _len = errors.length; _i < _len; _i++) {
						item = errors[_i];
						_results.push(this.errors.push(item));
					}
					return _results;
				};

				return ViewModel;

			})();

			exports.ViewModel = ViewModel;

		}).call(this);
	}, "invoice.list": function (exports, require, module) {
		(function () {
			var InvoiceRowViewModel, ViewModel,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				_this = this;

			InvoiceRowViewModel = (function () {

				function InvoiceRowViewModel(data) {
					this.undeleted = __bind(this.undeleted, this);
					this.undelete = __bind(this.undelete, this);
					this.showUrl = __bind(this.showUrl, this);
					this.destroy = __bind(this.destroy, this);
					var center, client, createdAt;
					this.model = data;
					this.InvoiceNo = data.get('InvoiceNo');
					this.Id = data.get('Id');
					client = data.get('Client');
					center = data.get('Center');
					this.CenterUrl = center.Url;
					if (client === void 0 || client === null) {
						this.Client = {
							FirstName: '',
							Category: '',
							LastName: '',
							Address: {
								City: '',
								CivicNumber: '',
								AptNumber: '',
								Street: '',
								PostalCode: ''
							}
						};
					} else {
						this.Client = client;
					}
					this.onUndeleted = null;
					createdAt = data.get('CreatedAt');
					this.CreatedAt = moment(createdAt).format('YYYY-MM-DD HH:mm');
				}

				InvoiceRowViewModel.prototype.destroy = function () {
					if (!confirm('êtes-vous sûr de vouloir supprimer la facture')) return;
					return this.model.destroy();
				};

				InvoiceRowViewModel.prototype.showUrl = function () {
					return '#invoice/show/' + this.Id;
				};

				InvoiceRowViewModel.prototype.undelete = function () {
					var data;
					data = {
						id: this.Id
					};
					return $.post('/invoice/undelete', data, this.undeleted);
				};

				InvoiceRowViewModel.prototype.undeleted = function (data) {
					if (this.onUndeleted == null) return;
					return this.onUndeleted(data);
				};

				return InvoiceRowViewModel;

			})();

			ViewModel = (function () {

				function ViewModel(model) {
					this.sort = __bind(this.sort, this);
					this.togglePreviousYears = __bind(this.togglePreviousYears, this);
					this.toggleCurrentYear = __bind(this.toggleCurrentYear, this);
					this.search = __bind(this.search, this);
					this.changePage = __bind(this.changePage, this);
					this.load = __bind(this.load, this);
					this.goToPage = __bind(this.goToPage, this);
					this.computeShowDateFields = __bind(this.computeShowDateFields, this);
					var vmItemFactory,
						_this = this;
					this.model = model;
					vmItemFactory = function (data) {
						var item;
						item = new InvoiceRowViewModel(data);
						item.onUndeleted = _this.load;
						return item;
					};
					this.items = kb.collectionObservable(this.model.invoices, {
						create: vmItemFactory
					});
					this.pageButtons = kb.collectionObservable(this.model.pageButtons);
					this.page = kb.observable(model, 'page');
					this.pageCount = kb.observable(model, 'pageCount');
					this.throttledPage = ko.computed(this.page).extend({
						throttle: 400
					});
					this.pageSize = kb.observable(model, 'pageSize');
					this.term = kb.observable(model, 'filterTerm');
					this.termFrom = kb.observable(model, 'filterTermFrom');
					this.termTo = kb.observable(model, 'filterTermTo');
					this.searchType = kb.observable(model, 'filterType');
					this.sortBy = kb.observable(model, 'sortBy');
					this.sortDir = kb.observable(model, 'sortDir');
					this.searchfocus = ko.observable(true);
					this.deleted = kb.observable(model, 'filterDeleted');
					this.isCurrentYear = kb.observable(model, 'CurrentYear');
					this.centerName = kb.observable(model, 'centerName');
					this.showDateFields = ko.observable(false);
					this.searchType.subscribe(this.computeShowDateFields);
					this.throttledPage.subscribe(this.goToPage);
				}

				ViewModel.prototype.computeShowDateFields = function () {
					var res;
					res = this.searchType() === "InvoiceDate";
					return this.showDateFields(res);
				};

				ViewModel.prototype.goToPage = function (val) {
					if (val && val > 1 && val < this.pageCount()) {
						return this.changePage({
							number: val
						});
					}
				};

				ViewModel.prototype.load = function () {
					return this.model.changePage(1);
				};

				ViewModel.prototype.changePage = function (vm) {
					var page;
					page = vm != null ? vm.number : 1;
					return this.model.changePage(page);
				};

				ViewModel.prototype.search = function () {
					return this.model.search();
				};

				ViewModel.prototype.toggleCurrentYear = function () {
					this.isCurrentYear(true);
					return this.load();
				};

				ViewModel.prototype.togglePreviousYears = function () {
					this.isCurrentYear(false);
					return this.load();
				};

				ViewModel.prototype.sort = function (sortBy) {
					if (sortBy !== this.sortBy()) {
						this.sortBy(sortBy);
						this.sortDir('desc');
					} else {
						this.sortDir(this.sortDir() === 'desc' ? 'asc' : 'desc');
					}
					return this.model.changePage(1);
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var model;
				model = new (require('invoice')).InvoiceList();
				return new ViewModel(model);
			};

		}).call(this);
	}, "invoice.show": function (exports, require, module) {
		(function () {
			var ViewModel, invoice,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			invoice = require('invoice');

			ViewModel = (function (_super) {

				__extends(ViewModel, _super);

				function ViewModel(model) {
					this.onPaymentError = __bind(this.onPaymentError, this);
					this.onPaymentComplete = __bind(this.onPaymentComplete, this);
					this.payWithCash = __bind(this.payWithCash, this);
					this.computeCreatedAt = __bind(this.computeCreatedAt, this);
					this.load = __bind(this.load, this); ViewModel.__super__.constructor.call(this, model);
					this.model = model;
					this.CreatedAtFormatted = ko.computed(this.computeCreatedAt);
				}

				ViewModel.prototype.load = function (id) {
					return this.model.fetchById(id);
				};

				ViewModel.prototype.computeCreatedAt = function () {
					var ca, res;
					ca = this.CreatedAt();
					res = ca.match(/\d+/g);
					if (res === null || res.length < 1) return '';
					res = res[0];
					ca = new Date(res * 1);
					return ca.toString('yyyy-MM-dd hh:mm:ss');
				};

				ViewModel.prototype.payWithCash = function () {
					return $.post("/payment/payWithCash?invoiceId=" + this.model.get("Id")).done(this.onPaymentComplete).fail(this.onPaymentError);
				};

				ViewModel.prototype.onPaymentComplete = function (result) {
					this.Payment = result.data;
					return alert("done");
				};

				ViewModel.prototype.onPaymentError = function () {
					return alert("error");
				};

				return ViewModel;

			})(kb.ViewModel);

			exports.createViewModel = function () {
				return new ViewModel(new invoice.InvoiceModel());
			};

		}).call(this);
	}, "invoice.workflow": function (exports, require, module) {
		(function () {
			var HubModel, InvoiceWorkflow,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; };

			HubModel = require('hub').Model;

			InvoiceWorkflow = (function () {

				function InvoiceWorkflow(navigate, remoteTemplate) {
					this.onReloaded = __bind(this.onReloaded, this);
					this.clientEdit = __bind(this.clientEdit, this);
					this.clientCreate = __bind(this.clientCreate, this);
					this.clientChange = __bind(this.clientChange, this);
					this.showOBNLForm = __bind(this.showOBNLForm, this);
					this.showForm = __bind(this.showForm, this);
					this.showForm2023 = __bind(this.showForm2023, this);
					this.showPocForm = __bind(this.showPocForm, this);
					this.showClientList = __bind(this.showClientList, this);
					this.newOBNLInvoice = __bind(this.newOBNLInvoice, this);
					this.newInvoice = __bind(this.newInvoice, this);
					this.newInvoice2023 = __bind(this.newInvoice2023, this);
					this.newPocInvoice = __bind(this.newPocInvoice, this);
					this.redirectIfNewInvoice = __bind(this.redirectIfNewInvoice, this); this.remoteTemplate = remoteTemplate;
					this.navigate = navigate;
					this.formViewModel = null;
					this.invoice = null;
				}

				InvoiceWorkflow.prototype.redirectIfNewInvoice = function (obnl, poc) {
					if (this.formViewModel === null) {
						if (!obnl && !poc) this.navigate('invoice/new');
						if (obnl && !poc) this.navigate('invoice/newOBNL');
						if (poc) this.navigate('invoice/new2023')
					}
					true;
					return false;
				};

				InvoiceWorkflow.prototype.newInvoice = function () {
					var hub, request,
						_this = this;
					this.invoice = new (require('invoice')).NewInvoiceModel();
					hub = new HubModel();
					request = hub.fetchCurrent().done(function () {
						var clientListVm;
						clientListVm = _this.showClientList(true, hub);
						clientListVm.reset();
						_this.formViewModel = new (require('invoice.form')).ViewModel(_this.invoice, clientListVm, false, hub);
						return _this.navigate('invoice/new/form');
					});
					return request;
				};

				InvoiceWorkflow.prototype.newInvoice2023 = function () {
					var hub, request,
						_this = this;
					this.invoice = new (require('invoice')).NewInvoiceModel1();
					hub = new HubModel();
					request = hub.fetchCurrent().done(function () {
						var clientListVm;
						clientListVm = _this.showClientList1(true, hub);
						clientListVm.reset();
						_this.formViewModel = new (require('invoice.form')).ViewModel(_this.invoice, clientListVm, false, hub);
						return _this.navigate('invoice/new/form2023');
					});
					return request;
				};

				InvoiceWorkflow.prototype.newPocInvoice = function () {
					var hub, request,
						_this = this;
					this.invoice = new (require('invoice')).NewInvoiceModel();
					hub = new HubModel();
					request = hub.fetchCurrent().done(function () {
						var clientListVm;
						clientListVm = _this.showClientList(true, hub);
						clientListVm.reset();
						_this.formViewModel = new (require('invoice.form')).ViewModel(_this.invoice, clientListVm, false, hub);
						return _this.navigate('invoice/newPocPage/form');
					});
					return request;
				};

				InvoiceWorkflow.prototype.newOBNLInvoice = function () {
					var clientListVm, hub, materials,
						_this = this;
					this.invoice = new (require('invoice')).NewInvoiceModel();
					materials = new (require('material')).ListModel();
					materials.filter = function (m) {
						return m.Active;
					};
					hub = new HubModel();
					hub.fetchCurrent();
					clientListVm = this.showClientList(false);
					clientListVm.reset();
					this.formViewModel = new (require('invoice.form')).ViewModel(this.invoice, materials, clientListVm, true, hub);
					return this.navigate('invoice/new/OBNLForm');
				};

				InvoiceWorkflow.prototype.showClientList = function (notOBNL) {
					var clientListVm,
						_this = this;
					clientListVm = new (require('client.list')).createViewModel();
					clientListVm.mode('pick');
					clientListVm.model.set({
						noCommercial: true,
						filterOBNLNumber: '',
						searchType: 'OBNLNumber'
					});
					if (!notOBNL) {
						clientListVm.model.set({
							filterCategory: 'OBNL'
						});
					} else {
						clientListVm.model.set({
							filterCategory: 'notOBNL'
						});
					}
					clientListVm.onNew = function () {
						return _this.navigate('invoice/new/client/create');
					};
					clientListVm.onEdit = function (vm) {
						return _this.navigate('invoice/new/client/edit/' + vm.Id());
					};
					clientListVm.onPick = function (client) {
						_this.invoice.selectClient(client.model());
						_this.formViewModel.showListPoc(false);
						return _this.formViewModel.showList(false);
					};
					return clientListVm;
				};

				InvoiceWorkflow.prototype.showClientList1 = function (notOBNL) {
					var clientListVm,
						_this = this;
					clientListVm = new (require('client.list1')).createViewModel();
					clientListVm.mode('pick');
					clientListVm.model.set({
						noCommercial: true,
						filterOBNLNumber: '',
						searchType: 'OBNLNumber'
					});
					if (!notOBNL) {
						clientListVm.model.set({
							filterCategory: 'OBNL'
						});
					} else {
						clientListVm.model.set({
							filterCategory: 'notOBNL'
						});
					}
					clientListVm.onNew = function () {
						return _this.navigate('invoice/new/client/create');
					};
					clientListVm.onEdit = function (vm) {
						return _this.navigate('invoice/new/client/edit/' + vm.Id());
					};
					clientListVm.onPick = function (client) {
						_this.invoice.selectClient(client.model());
						_this.formViewModel.showListPoc(false);
						return _this.formViewModel.showList(false);
					};
					return clientListVm;
				};

				InvoiceWorkflow.prototype.showForm = function () {
					var _ref;
					if ((_ref = this.formViewModel) != null ? _ref.saved : void 0) {
						this.navigate('invoice/new');
					}
					if (this.redirectIfNewInvoice(false, false)) return;
					this.formViewModel.invoicePreview(null);
					return this.remoteTemplate('invoice_newTemplate', this.formViewModel);
				};

				InvoiceWorkflow.prototype.showForm2023 = function () {
					var _ref;
					if ((_ref = this.formViewModel) != null ? _ref.saved : void 0) {
						this.navigate('invoice/new2023');
					}
					if (this.redirectIfNewInvoice(false, true)) return;
					this.formViewModel.invoicePreview(null);
					return this.remoteTemplate('invoice_newTemplate2023', this.formViewModel);
				};

				InvoiceWorkflow.prototype.showPocForm = function () {
					var _ref;
					if ((_ref = this.formViewModel) != null ? _ref.saved : void 0) {
						this.navigate('invoice/newPocPage');
					}
					if (this.redirectIfNewInvoice(false, true)) return;
					this.formViewModel.invoicePreview(null);
					return this.remoteTemplate('invoice_newPocPageTemplate', this.formViewModel);
				};

				InvoiceWorkflow.prototype.showOBNLForm = function () {
					var _ref;
					if ((_ref = this.formViewModel) != null ? _ref.saved : void 0) {
						this.navigate('invoice/newOBNL');
					}
					if (this.redirectIfNewInvoice(true)) return;
					this.formViewModel.invoicePreview(null);
					return this.remoteTemplate('invoice_newOBNLTemplate', this.formViewModel);
				};

				InvoiceWorkflow.prototype.clientChange = function () {
					var _this = this;
					if (this.redirectIfNewInvoice()) return;
					return this.showClientList(function (cvm) {
						return cvm.onCancel = function () {
							return _this.navigate('invoice/new/form');
						};
					});
				};

				InvoiceWorkflow.prototype.clientCreate = function () {
					var clientFormVM,
						_this = this;
					if (this.redirectIfNewInvoice()) return;
					clientFormVM = new (require('client.form')).createViewModel();
					clientFormVM.onCancel = function () {
						return _this.navigate('invoice/new/client/change');
					};
					clientFormVM.loadNew();
					this.remoteTemplate('client_newTemplate', clientFormVM);
					return clientFormVM.onSaved = function (client) {
						return client.fetch({
							reset: true,
							success: _this.onReloaded
						});
					};
				};

				InvoiceWorkflow.prototype.clientEdit = function (id) {
					var clientFormVM,
						_this = this;
					if (this.redirectIfNewInvoice()) return;
					clientFormVM = new (require('client.form')).createViewModel();
					clientFormVM.onCancel = function () {
						return _this.navigate('invoice/new/client/change');
					};
					this.remoteTemplate('client_newTemplate', clientFormVM);
					clientFormVM.load(id);
					return clientFormVM.onSaved = function (client) {
						return client.fetch({
							reset: true,
							success: _this.onReloaded
						});
					};
				};

				InvoiceWorkflow.prototype.onReloaded = function (client) {
					this.invoice.selectClient(client);
					this.formViewModel.showList(false);
					this.formViewModel.showListPoc(false);
					this.navigate('invoice/new/form');
					return false;
				};

				return InvoiceWorkflow;

			})();

			exports.Workflow = InvoiceWorkflow;

		}).call(this);
	}, "jquery.ext": function (exports, require, module) {
		function setup() {
			$.fn.loading = function () {
				this.data("loading-text", "<i class='fa fa-spinner fa-spin'></i>  " + this.html()).button("loading");
			};

			$.fn.stopLoading = function () {
				this.button("reset");
			};
		}
		exports.setup = setup
	}, "jqueryui": function (exports, require, module) {
		(function () {
			var Dialog,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; };

			Dialog = (function () {

				function Dialog() {
					this.close = __bind(this.close, this);
					this.open = __bind(this.open, this); this.isVisible = ko.observable(false);
					this.settings = {
						autoOpen: false,
						close: this.close
					};
				}

				Dialog.prototype.open = function () {
					return this.isVisible(true);
				};

				Dialog.prototype.close = function () {
					return this.isVisible(false);
				};

				return Dialog;

			})();

			exports.Dialog = Dialog;

		}).call(this);
	}, "ko.ext": function (exports, require, module) {
		(function () {
			var LocaleManager, delay, setup,
				_this = this;

			LocaleManager = (function () {

				function LocaleManager(locale_identifier, translations_by_locale) {
					this.translations_by_locale = translations_by_locale;
					this.current_locale = ko.observable(locale_identifier);
				}

				LocaleManager.prototype.get = function (string_id) {
					if (!this.translations_by_locale[this.current_locale()]) return string_id;
					if (!this.translations_by_locale[this.current_locale()].hasOwnProperty(string_id)) {
						return string_id;
					}
					return this.translations_by_locale[this.current_locale()][string_id];
				};

				LocaleManager.prototype.getLocale = function () {
					return this.current_locale();
				};

				LocaleManager.prototype.setLocale = function (locale_identifier) {
					this.current_locale(locale_identifier);
					return this.trigger('change', this);
				};

				return LocaleManager;

			})();

			_.extend(LocaleManager.prototype, Backbone.Events);

			delay = function (ms, func) {
				return setTimeout(func, ms);
			};

			setup = function () {
				var moneyHandler, toMoney;
				$.get('/default/localizations', {}, function (data) {
					return kb.locale_manager = new LocaleManager('frCA', data);
				});
				ko.bindingHandlers.dialog = {
					init: function (element, valueAccessor) {
						var dialog;
						dialog = ko.utils.unwrapObservable(valueAccessor());
						$(element).dialog(dialog.settings);
						return dialog.isVisible.subscribe(function (iv) {
							var action;
							action = iv ? 'open' : 'close';
							return $(element).dialog(action);
						});
					}
				};
				ko.bindingHandlers.modal = {
					init: function (element, valueAccessor) {
						var dialog, initialAction;
						dialog = ko.utils.unwrapObservable(valueAccessor());
						$(element).modal();
						$(element).on('shown', function () {
							dialog.isVisible(true);
							return dialog.onShown();
						});
						$(element).on('hidden', function () {
							return dialog.isVisible(false);
						});
						initialAction = dialog.isVisible() ? 'show' : 'hide';
						$(element).modal('hide');
						$(element).delay(200).queue(function (t) {
							return $(element).modal(initialAction);
						});
						return dialog.isVisible.subscribe(function (iv) {
							var action;
							action = iv ? 'show' : 'hide';
							return $(element).modal(action);
						});
					}
				};
				ko.bindingHandlers.onKeyEnter = {
					init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
						var callback;
						callback = ko.utils.unwrapObservable(valueAccessor());
						return $(element).keypress(function (event) {
							var keyCode;
							keyCode = event.which;
							if (!event.which) keyCode = event.keyCode;
							if (keyCode === 13) {
								callback.call(viewModel);
								return false;
							}
							return true;
						});
					}
				};
				ko.bindingHandlers.file = {
					init: function (element, valueAccessor) {
						var fu, handler;
						handler = ko.utils.unwrapObservable(valueAccessor());
						return fu = $(element).fileupload(handler);
					}
				};
				ko.bindingHandlers.datepicker = {
					init: function (element, valueAccessor, allBindingsAccessor) {
						var input;
						input = $(element);
						input.datepicker();
						input.change(function () {
							var newDate, res;
							newDate = input.datepicker('getDate');
							return res = valueAccessor()(newDate);
						});
						return ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
							return $(element).datepicker("destroy");
						});
					},
					update: function (element, valueAccessor) {
						var value;
						value = ko.utils.unwrapObservable(valueAccessor());
						return $(element).datepicker("setDate", value);
					}
				};
				ko.bindingHandlers.filterHyphens = {
					init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
						$(element).keypress(function (event) {
							var keyCode;
							keyCode = event.which;
							if (!event.which) keyCode = event.keyCode;
							if (keyCode === 45) {
								delay(0, function () {
									return $(element).val($(element).val().replace(/-+/g, ' '));
								});
							}
							return true;
						});
						return $(element).on('paste', function (event) {
							var pastedText;
							if (window.clipboardData && window.clipboardData.getData('Text')) {
								pastedText = window.clipboardData.getData('Text');
							} else {
								pastedText = event.originalEvent.clipboardData.getData('text/plain');
							}
							if (/-/.test(pastedText)) {
								delay(0, function () {
									return $(element).val($(element).val().replace(/-+/g, ' ').trim());
								});
							}
							return true;
						});
					}
				};
				ko.bindingHandlers.chart = {
					init: function (element, valueAccessor, allBindingsAccessor) { },
					update: function (element, valueAccessor) {
						var ctx, data, options;
						data = ko.utils.unwrapObservable(valueAccessor());
						if (data === null) return;
						element.style.width = '100%';
						element.style.height = '100%';
						element.width = element.offsetWidth;
						element.height = element.offsetHeight;
						ctx = element.getContext("2d");
						options = {};
						return new Chart(ctx).Bar(data);
					}
				};
				ko.bindingHandlers.autocomplete = {
					init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
						var options, value;
						value = valueAccessor();
						options = {
							select: value.select,
							change: value.change,
							source: value.source,
							focus: function () {
								return false;
							},
							minLength: 0
						};
						return $(element).autocomplete(options);
					}
				};
				moneyHandler = function (element, valueAccessor, allBindings) {
					var $el, method, valueUnwrapped;
					$el = $(element);
					valueUnwrapped = ko.unwrap(valueAccessor());
					if ($el.is(':input')) {
						method = 'val';
					} else {
						method = 'text';
					}
					return $el[method](toMoney(valueUnwrapped));
				};
				ko.bindingHandlers.money = {
					update: moneyHandler
				};
				return toMoney = function (num) {
					return '$' + (Number(num).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
				};
			};

			exports.setup = setup;

		}).call(this);
	}, "limits": function (exports, require, module) {
		(function () {
			var LimitRow, Model,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

			LimitRow = (function () {

				function LimitRow(data) {
					this.Name = data.MaterialName;
					this.Quantity = data.QuantitySoFar;
					this.MaxYearlyAmount = data.MaxQuantity;
					this.Unit = data.Unit;
					this.IsExcluded = data.IsExcluded;
					this.BaseProgress = (this.Quantity / this.MaxYearlyAmount) * 100;
					if (this.BaseProgress > 100) this.BaseProgress = 100;
				}

				return LimitRow;

			})();

			Model = (function (_super) {

				__extends(Model, _super);

				Model.prototype.url = 'limits';

				Model.prototype.idAttribute = 'Id';

				function Model() {
					this.fetchById = __bind(this.fetchById, this);
					this.fetchByClientId = __bind(this.fetchByClientId, this);
					this.parse = __bind(this.parse, this); this.CurrentLimits = new Backbone.Collection();
					Model.__super__.constructor.call(this, {});
				}

				Model.prototype.parse = function (resp) {
					var limit, _i, _len, _ref, _results;
					this.CurrentLimits.reset();
					_ref = resp.CurrentLimits;
					_results = [];
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						limit = _ref[_i];
						_results.push(this.CurrentLimits.push(new LimitRow(limit)));
					}
					return _results;
				};

				Model.prototype.fetchByClientId = function (id) {
					return this.fetch({
						data: {
							clientId: id
						}
					});
				};

				Model.prototype.fetchById = function (id) {
					return this.fetch({
						data: {
							id: id
						}
					});
				};

				return Model;

			})(Backbone.Model);

			exports.Model = Model;

		}).call(this);
	}, "login": function (exports, require, module) {
		(function () {
			var LoginViewModel,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				_this = this;

			LoginViewModel = (function () {

				function LoginViewModel() {
					this.onError = __bind(this.onError, this);
					this.submit = __bind(this.submit, this); this.login = ko.observable();
					this.password = ko.observable();
					this.processing = ko.observable(false);
					this.errors = ko.observableArray([]);
				}

				LoginViewModel.prototype.submit = function () {
					var data,
						_this = this;
					this.processing(true);
					this.errors.removeAll();
					data = {
						login: this.login(),
						password: this.password()
					};
					return $.ajax({
						type: "POST",
						url: '/user/login',
						data: data,
						success: function () {
							_this.processing(false);
							return window.location.pathname = '';
						},
						error: this.onError
					});
				};

				LoginViewModel.prototype.onError = function (errors, data) {
					var item, _i, _len, _results;
					this.processing(false);
					errors = $.parseJSON(errors.responseText);
					this.errors.removeAll();
					_results = [];
					for (_i = 0, _len = errors.length; _i < _len; _i++) {
						item = errors[_i];
						_results.push(this.errors.push(item));
					}
					return _results;
				};

				return LoginViewModel;

			})();

			exports.setup = function () {
				var vm;
				vm = new LoginViewModel();
				return ko.applyBindings(vm, $('#login-form')[0]);
			};

		}).call(this);
	}, "material": function (exports, require, module) {
		(function () {
			var ListModel, MaterialModel,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

			MaterialModel = (function (_super) {

				__extends(MaterialModel, _super);

				MaterialModel.prototype.url = 'material';

				MaterialModel.prototype.idAttribute = 'Id';

				function MaterialModel() {
					this.include = __bind(this.include, this);
					this.exclude = __bind(this.exclude, this);
					this.disable = __bind(this.disable, this);
					this.enable = __bind(this.enable, this); MaterialModel.__super__.constructor.call(this, {
						Id: null,
						Name: '',
						Tag: '',
						Unit: '',
						Price: 0,
						Active: false,
						MaxYearlyAmount: 100,
						IsExcluded: false
					});
				}

				MaterialModel.prototype.enable = function (vm) {
					this.set({
						Active: true
					});
					return this.save();
				};

				MaterialModel.prototype.disable = function (vm) {
					this.set({
						Active: false
					});
					return this.save();
				};

				MaterialModel.prototype.exclude = function (vm) {
					this.set({
						IsExcluded: true
					});
					return this.save();
				};

				MaterialModel.prototype.include = function (vm) {
					this.set({
						IsExcluded: false
					});
					return this.save();
				};

				return MaterialModel;

			})(Backbone.Model);

			ListModel = (function (_super) {

				__extends(ListModel, _super);

				ListModel.prototype.url = 'material';

				ListModel.prototype.filter = null;

				function ListModel() {
					this.parse = __bind(this.parse, this);
					this.fetch = __bind(this.fetch, this); ListModel.__super__.constructor.call(this, {
						term: ''
					});
					this.materials = new Backbone.Collection();
				}

				ListModel.prototype.fetch = function (options) {
					options = options || {};
					return ListModel.__super__.fetch.call(this, {
						data: {
							term: this.get('term'),
							onlyCurrentHub: options.onlyCurrentHub,
							municipality: options.municipality
						}
					});
				};

				ListModel.prototype.parse = function (resp) {
					var item, model, _i, _len, _results;
					this.materials.reset();
					_results = [];
					for (_i = 0, _len = resp.length; _i < _len; _i++) {
						item = resp[_i];
						if (this.filter !== null && !this.filter(item)) continue;
						model = new MaterialModel();
						model.set(item);
						_results.push(this.materials.push(model));
					}
					return _results;
				};

				return ListModel;

			})(Backbone.Model);

			exports.ListModel = ListModel;

		}).call(this);
	}, "material.form": function (exports, require, module) {
		(function () {
			var MaterialModel, ViewModel,
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				_this = this;

			MaterialModel = (function (_super) {

				__extends(MaterialModel, _super);

				function MaterialModel() {
					this.url = 'material';
					this.idAttribute = 'Id';
					MaterialModel.__super__.constructor.call(this, {
						Id: null,
						Name: '',
						Tag: '',
						Unit: '',
						Price: 0,
						MaxYearlyAmount: 100,
						IsExcluded: false
					});
				}

				return MaterialModel;

			})(Backbone.Model);

			ViewModel = (function () {

				function ViewModel(model) {
					this.onSaved = __bind(this.onSaved, this);
					this.save = __bind(this.save, this);
					this.loadNew = __bind(this.loadNew, this);
					this.load = __bind(this.load, this);
					this.isNew = __bind(this.isNew, this); this.model = model;
					this.id = kb.observable(model, 'Id');
					this.tag = kb.observable(model, 'Tag');
					this.name = kb.observable(model, 'Name');
					this.price = kb.observable(model, 'Price');
					this.unit = kb.observable(model, 'Unit');
					this.maxYearlyAmount = kb.observable(model, 'MaxYearlyAmount');
					this.isExcluded = kb.observable(model, 'IsExcluded');
				}

				ViewModel.prototype.isNew = function () {
					return this.id() === null;
				};

				ViewModel.prototype.load = function (id) {
					return this.model.fetch({
						data: {
							id: id
						}
					});
				};

				ViewModel.prototype.loadNew = function () {
					this.globalSettingsModel = new (require('globalsettings')).Model();
					this.globalSettingsModel.fetch({
						async: false
					});
					return this.unit(this.globalSettingsModel.get('DefaultMaterialUnit'));
				};

				ViewModel.prototype.save = function () {
					return this.model.save(null, {
						success: this.onSaved
					});
				};

				ViewModel.prototype.onSaved = function () {
					eco.app.notifications.addNotification('msg-material-saved-successfully');
					return window.location.hash = 'material/index';
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				return new ViewModel(new MaterialModel());
			};

		}).call(this);
	}, "material.list": function (exports, require, module) {
		(function () {
			var ViewModel, material,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				_this = this;

			material = require('material');

			ViewModel = (function () {

				function ViewModel(model) {
					this.editUrl = __bind(this.editUrl, this);
					this.load = __bind(this.load, this);
					this.filterNotExcluded = __bind(this.filterNotExcluded, this);
					this.filterExcluded = __bind(this.filterExcluded, this);
					this.filterInactive = __bind(this.filterInactive, this);
					this.filterActive = __bind(this.filterActive, this);
					this.verified = __bind(this.verified, this);
					this.noverified = __bind(this.noverified, this);
					this.search = __bind(this.search, this); this.model = model;
					this.materials = kb.collectionObservable(this.model.materials);
					this.materialsActive = ko.computed(this.filterActive);
					this.materialsInactive = ko.computed(this.filterInactive);
					this.materialIsExcluded = ko.computed(this.filterNotExcluded);
					this.materialIsNotExcluded = ko.computed(this.filterExcluded);
					this.term = kb.observable(model, 'term');
					this.searchfocus = ko.observable(true);
				}

				ViewModel.prototype.search = function () {
					return this.model.fetch();
				};

				ViewModel.prototype.filterActive = function () {
					var _this = this;
					return ko.utils.arrayFilter(this.materials(), function (item) {
						return item.Active();
					});
				};

				ViewModel.prototype.filterInactive = function () {
					var _this = this;
					return ko.utils.arrayFilter(this.materials(), function (item) {
						return !item.Active();
					});
				};

				ViewModel.prototype.filterExcluded = function () {
					var _this = this;
					return ko.utils.arrayFilter(this.materials(), function (item) {
						return item.IsExcluded();
					});
				};

				ViewModel.prototype.filterNotExcluded = function () {
					var _this = this;
					return ko.utils.arrayFilter(this.materials(), function (item) {
						return !item.IsExcluded();
					});
				};

				ViewModel.prototype.load = function () {
					return this.model.fetch();
				};

				ViewModel.prototype.editUrl = function (m) {
					return '#material/edit/' + m.Id();
				};

				ViewModel.prototype.enable = function (vm) {
					return vm.model().enable();
				};

				ViewModel.prototype.disable = function (vm) {
					return vm.model().disable();
				};

				ViewModel.prototype.exclude = function (vm) {
					return vm.model().exclude();
				};

				ViewModel.prototype.include = function (vm) {
					return vm.model().include();
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var model;
				model = new material.ListModel();
				return new ViewModel(model);
			};

		}).call(this);
	}, "material.merger": function (exports, require, module) {
		(function () {
			var MaterialMergerModel, MaterialMergerWorkflow, ViewModel, material,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			material = require('material');

			MaterialMergerModel = (function (_super) {

				__extends(MaterialMergerModel, _super);

				function MaterialMergerModel() {
					this.resetMergeDest = __bind(this.resetMergeDest, this);
					this.save = __bind(this.save, this);
					this.removeMergeSource = __bind(this.removeMergeSource, this);
					this.addMergeSource = __bind(this.addMergeSource, this);
					this.selectMergeDest = __bind(this.selectMergeDest, this); this.url = 'material/merge';
					MaterialMergerModel.__super__.constructor.call(this, {
						MergeDest: null,
						MergeSources: []
					});
				}

				MaterialMergerModel.prototype.selectMergeDest = function (material) {
					return this.set({
						MergeDest: material.Id()
					});
				};

				MaterialMergerModel.prototype.addMergeSource = function (material) {
					var curMergeSources;
					curMergeSources = this.get('MergeSources');
					if (!~curMergeSources.indexOf(material.Id())) {
						curMergeSources.push(material.Id());
						material.material_merger_materialWasPicked(true);
						return true;
					}
					return false;
				};

				MaterialMergerModel.prototype.removeMergeSource = function (material) {
					var curMergeSources, index;
					curMergeSources = this.get('MergeSources');
					index = curMergeSources.indexOf(material.Id());
					if (~index) {
						curMergeSources.splice(index, 1);
						material.material_merger_materialWasPicked(false);
					}
					return index;
				};

				MaterialMergerModel.prototype.save = function () {
					var mergeSourcesString;
					mergeSourcesString = this.get('MergeSources').join(',');
					this.set({
						MergeSourcesStr: mergeSourcesString
					});
					return MaterialMergerModel.__super__.save.apply(this, arguments).save;
				};

				MaterialMergerModel.prototype.resetMergeDest = function (material) {
					if (material.Id() === this.get('MergeDest')) {
						this.set({
							MergeDest: null
						});
						return true;
					}
					return false;
				};

				return MaterialMergerModel;

			})(Backbone.Model);

			ViewModel = (function (_super) {

				__extends(ViewModel, _super);

				function ViewModel(model, materials) {
					this.removeMaterialNameFromHeaderElement = __bind(this.removeMaterialNameFromHeaderElement, this);
					this.addMaterialNameToHeaderElement = __bind(this.addMaterialNameToHeaderElement, this);
					this.changeDest = __bind(this.changeDest, this);
					this.onRemoveSrcPick = __bind(this.onRemoveSrcPick, this);
					this.onError = __bind(this.onError, this);
					this.onMerged = __bind(this.onMerged, this);
					this.merge = __bind(this.merge, this);
					this.load = __bind(this.load, this); this.model = model;
					ViewModel.__super__.constructor.call(this, model);
					this.errors = ko.observableArray([]);
					this.materials = materials;
					this.mergeDest = ko.observable(null);
					this.mergeSources = ko.observableArray();
					this.showDestList = ko.observable(true);
					this.materials.load();
				}

				ViewModel.prototype.load = function () {
					return this.model().fetch();
				};

				ViewModel.prototype.merge = function () {
					if (!this.mergeDest() || !this.mergeSources().length) {
						alert("Se il vous pla�t, s�lectionnez les deux fusionnent sources et fusionner destination de continuer.");
						return;
					}
					if (!confirm("Attention! L'op�ration est irr�versible! Etes-vous s�r de vouloir continuer?")) {
						return;
					}
					$("#global-loading-fade").modal('show');
					return this.model().save(null, {
						success: this.onMerged,
						error: this.onError
					});
				};

				ViewModel.prototype.onMerged = function () {
					var setTimeoutCallback;
					setTimeoutCallback = function () {
						return $("#global-loading-fade").modal('hide');
					};
					setTimeout(setTimeoutCallback, 500);
					eco.app.notifications.addNotification('msg-material-merger-run-successfully');
					return eco.app.router.navigate('material/index', {
						trigger: true
					});
				};

				ViewModel.prototype.onError = function (m, errors, data) {
					var item, setTimeoutCallback, _i, _len, _results;
					setTimeoutCallback = function () {
						return $("#global-loading-fade").modal('hide');
					};
					setTimeout(setTimeoutCallback, 500);
					errors = $.parseJSON(errors.responseText);
					this.errors.removeAll();
					eco.app.notifications.removeNotification('msg-material-merger-run-successfully');
					_results = [];
					for (_i = 0, _len = errors.length; _i < _len; _i++) {
						item = errors[_i];
						_results.push(this.errors.push(item));
					}
					return _results;
				};

				ViewModel.prototype.onRemoveSrcPick = function (material) {
					var index;
					index = this.model().removeMergeSource(material);
					if (~index) {
						this.mergeSources.splice(index, 1);
						return this.removeMaterialNameFromHeaderElement($('#merge-sources-names'), material);
					}
				};

				ViewModel.prototype.changeDest = function () {
					return this.showDestList(true);
				};

				ViewModel.prototype.addMaterialNameToHeaderElement = function ($element, material, replace) {
					var elementText, materialFullName;
					elementText = $element.text();
					materialFullName = material.Name() + ' (' + material.Tag() + ')';
					if (!replace) {
						return $element.text(elementText && elementText !== '-' ? elementText + ', ' + materialFullName : materialFullName);
					} else {
						return $element.text(materialFullName);
					}
				};

				ViewModel.prototype.removeMaterialNameFromHeaderElement = function ($element, material) {
					var elementText, materialFullName, removalRegExp;
					elementText = $element.text();
					materialFullName = material.Name() + ' (' + material.Tag() + ')';
					removalRegExp = new RegExp('(,\\s)?' + materialFullName.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'));
					elementText = elementText.replace(removalRegExp, '');
					elementText = elementText.replace(/^,\s/g, '');
					if (!elementText) elementText = '-';
					return $element.text(elementText);
				};

				return ViewModel;

			})(Knockback.ViewModel);

			MaterialMergerWorkflow = (function () {

				function MaterialMergerWorkflow() {
					this.showMaterialList = __bind(this.showMaterialList, this);
					this.runMerger = __bind(this.runMerger, this); this.formViewModel = null;
					this.merger = null;
				}

				MaterialMergerWorkflow.prototype.runMerger = function () {
					var materials,
						_this = this;
					this.merger = new MaterialMergerModel();
					materials = this.showMaterialList();
					materials.model.materials.bind('add', function (item) {
						return item.set({
							material_merger_materialWasPicked: false
						});
					});
					return this.formViewModel = new ViewModel(this.merger, materials);
				};

				MaterialMergerWorkflow.prototype.showMaterialList = function (setup) {
					var materialListVm,
						_this = this;
					if (setup == null) setup = null;
					materialListVm = new (require('material.list')).createViewModel();
					materialListVm.onDestPick = function (material) {
						_this.merger.selectMergeDest(material);
						_this.formViewModel.mergeDest(material);
						_this.formViewModel.showDestList(false);
						return _this.formViewModel.addMaterialNameToHeaderElement($('#merge-dest-names'), material, true);
					};
					materialListVm.onSrcPick = function (material) {
						if (_this.merger.addMergeSource(material)) {
							_this.formViewModel.mergeSources.push(material);
							if (_this.merger.resetMergeDest(material)) {
								_this.formViewModel.changeDest;
								_this.formViewModel.removeMaterialNameFromHeaderElement($('#merge-dest-names'), material);
							}
							return _this.formViewModel.addMaterialNameToHeaderElement($('#merge-sources-names'), material);
						}
					};
					return materialListVm;
				};

				return MaterialMergerWorkflow;

			})();

			exports.createViewModel = function () {
				var workflow;
				workflow = new MaterialMergerWorkflow();
				return workflow.runMerger();
			};

		}).call(this);
	}, "municipality": function (exports, require, module) {
		(function () {
			var MunicipalityListModel, MunicipalityModel,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

			MunicipalityModel = (function (_super) {

				__extends(MunicipalityModel, _super);

				MunicipalityModel.prototype.url = 'municipality';

				function MunicipalityModel() {
					this.disable = __bind(this.disable, this);
					this.enable = __bind(this.enable, this); this.url = 'municipality';
					this.idAttribute = 'Id';
					MunicipalityModel.__super__.constructor.call(this, {
						Id: null,
						Name: '',
						Enabled: false
					});
				}

				MunicipalityModel.prototype.enable = function (vm) {
					this.set({
						Enabled: true
					});
					return this.save();
				};

				MunicipalityModel.prototype.disable = function (vm) {
					this.set({
						Enabled: false
					});
					return this.save();
				};

				return MunicipalityModel;

			})(Backbone.Model);

			MunicipalityListModel = (function (_super) {

				__extends(MunicipalityListModel, _super);

				function MunicipalityListModel() {
					this.parse = __bind(this.parse, this);
					MunicipalityListModel.__super__.constructor.apply(this, arguments);
				}

				MunicipalityListModel.prototype.url = 'municipality';

				MunicipalityListModel.prototype.municipalities = new Backbone.Collection();

				MunicipalityListModel.prototype.filter = null;

				MunicipalityListModel.prototype.parse = function (resp) {
					var item, row, _i, _len;
					this.municipalities.reset();
					for (_i = 0, _len = resp.length; _i < _len; _i++) {
						item = resp[_i];
						if (this.filter !== null && !this.filter(item)) continue;
						row = new MunicipalityModel();
						row.set(item);
						this.municipalities.push(row);
					}
					return this.trigger('reloaded');
				};

				return MunicipalityListModel;

			})(Backbone.Model);

			exports.Model = MunicipalityModel;

			exports.ListModel = MunicipalityListModel;

		}).call(this);
	}, "municipality.form": function (exports, require, module) {
		(function () {
			var ViewModel,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				_this = this;

			ViewModel = (function () {

				function ViewModel(model) {
					this.save = __bind(this.save, this);
					this.load = __bind(this.load, this);
					this.updateNew = __bind(this.updateNew, this); this.model = model;
					this.id = kb.observable(model, 'Id');
					this.name = kb.observable(model, 'Name');
					this.isNew = ko.observable(true);
					model.on('change', this.updateNew);
				}

				ViewModel.prototype.updateNew = function () {
					return this.isNew(this.model.isNew());
				};

				ViewModel.prototype.load = function (id) {
					return this.model.fetch({
						data: {
							id: id
						}
					});
				};

				ViewModel.prototype.save = function () {
					this.model.save();
					return window.location.hash = 'municipality/index';
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var model, module;
				module = require('municipality');
				model = new module.Model();
				return new ViewModel(model);
			};

		}).call(this);
	}, "municipality.list": function (exports, require, module) {
		(function () {
			var ViewModel,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				_this = this;

			ViewModel = (function () {

				function ViewModel(model) {
					this.editUrl = __bind(this.editUrl, this);
					this.load = __bind(this.load, this);
					this.filterInactive = __bind(this.filterInactive, this);
					this.filterActive = __bind(this.filterActive, this); this.model = model;
					this.verified = __bind(this.verified, this); this.model = model;
					this.noverified = __bind(this.noverified, this); this.model = model;
					this.municipalities = kb.collectionObservable(this.model.municipalities);
					this.municipalitiesActive = ko.computed(this.filterActive);
					this.municipalitiesInactive = ko.computed(this.filterInactive);
				}

				ViewModel.prototype.filterActive = function () {
					var _this = this;
					return ko.utils.arrayFilter(this.municipalities(), function (item) {
						return item.Enabled();
					});
				};

				ViewModel.prototype.filterInactive = function () {
					var _this = this;
					return ko.utils.arrayFilter(this.municipalities(), function (item) {
						return !item.Enabled();
					});
				};

				ViewModel.prototype.load = function () {
					return this.model.fetch();
				};

				ViewModel.prototype.editUrl = function (m) {
					return '#municipality/edit/' + m.Id();
				};

				ViewModel.prototype.enable = function (vm) {
					return vm.model().enable();
				};

				ViewModel.prototype.disable = function (vm) {
					return vm.model().disable();
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var model;
				model = new (require('municipality')).ListModel();
				return new ViewModel(model);
			};

		}).call(this);
	}, "obnlreinvestment": function (exports, require, module) {
		(function () {
			var NewOBNLReinvestmentModel, OBNLNumberAutocompleteViewModel, OBNLReinvestmentModel, OBNLReinvestmentsList, common,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

			common = require('common');

			OBNLNumberAutocompleteViewModel = (function () {

				function OBNLNumberAutocompleteViewModel(value) {
					this.source = __bind(this.source, this);
					this.change = __bind(this.change, this);
					this.select = __bind(this.select, this); this.value = value;
				}

				OBNLNumberAutocompleteViewModel.prototype.select = function (e, i) {
					this.value(i.item.label);
					return false;
				};

				OBNLNumberAutocompleteViewModel.prototype.change = function (e) {
					return false;
				};

				OBNLNumberAutocompleteViewModel.prototype.source = function (request, response) {
					var data, url,
						_this = this;
					url = '/client/suggestobnl';
					data = {
						term: request.term
					};
					return $.get(url, data, function (cat)
					{
						var arr = new Set(cat);
						cat = [...arr];
						if (!cat.length)
						{
							cat = ["No result"]
						}
						return response(cat);
					});
				};

				return OBNLNumberAutocompleteViewModel;

			})();

			OBNLReinvestmentsList = (function (_super) {

				__extends(OBNLReinvestmentsList, _super);

				OBNLReinvestmentsList.prototype.obnlReinvestments = new Backbone.Collection();

				OBNLReinvestmentsList.prototype.pageButtons = new Backbone.Collection();

				OBNLReinvestmentsList.prototype.urlRoot = '/obnlreinvestment/index';

				function OBNLReinvestmentsList() {
					this.changePage = __bind(this.changePage, this);
					this.search = __bind(this.search, this);
					this.parse = __bind(this.parse, this);
					this.loadForUser = __bind(this.loadForUser, this); this.obnlReinvestments.url = '/obnlreinvestment/index';
					OBNLReinvestmentsList.__super__.constructor.call(this, {
						filterTerm: '',
						filterTermFrom: null,
						filterTermTo: null,
						filterType: 'obnlReinvestmentNo',
						filterDeleted: false,
						CurrentYear: true,
						sortBy: 'obnlReinvestmentDate',
						sortDir: 'desc',
						userId: '',
						page: 1,
						pageCount: 1,
						pageSize: null,
						centerName: 'Tous',
						total: 0
					});
				}

				OBNLReinvestmentsList.prototype.loadForUser = function (id) {
					this.set('userId', id);
					return this.search();
				};

				OBNLReinvestmentsList.prototype.parse = function (resp) {
					var item, model, page, pages, _i, _j, _len, _len2, _ref, _results;
					this.obnlReinvestments.reset();
					this.set('total', resp.Total);
					_ref = resp.OBNLReinvestments;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						model = new OBNLReinvestmentModel;
						model.set(item);
						this.obnlReinvestments.push(model);
					}
					this.pageButtons.reset();
					pages = common.generatePages(resp.Page, resp.PageCount);
					this.set('page', resp.Page);
					this.set('pageCount', resp.PageCount);
					_results = [];
					for (_j = 0, _len2 = pages.length; _j < _len2; _j++) {
						page = pages[_j];
						_results.push(this.pageButtons.push(page));
					}
					return _results;
				};

				OBNLReinvestmentsList.prototype.search = function () {
					return this.changePage(1);
				};

				OBNLReinvestmentsList.prototype.changePage = function (page) {
					var $loadingFade, fetchAjax, from, to,
						_this = this;
					this.set('page', page);
					from = this.get('filterTermFrom');
					to = this.get('filterTermTo');
					if (from != null) from = from.toString('yyyy-MM-dd');
					if (to != null) to = to.toString('yyyy-MM-dd');
					$loadingFade = $("#global-loading-fade");
					$loadingFade.modal('show');
					fetchAjax = this.fetch({
						data: {
							page: this.get('page'),
							pageSize: this.get('pageSize'),
							userId: this.get('userId'),
							sortDir: this.get('sortDir'),
							sortBy: this.get('sortBy'),
							CurrentYear: this.get('CurrentYear'),
							Deleted: this.get('filterDeleted'),
							Type: this.get('filterType'),
							Term: this.get('filterTerm'),
							TermFrom: from,
							TermTo: to,
							CenterName: this.get('centerName')
						}
					});
					return fetchAjax.complete(function () {
						var setTimeoutCallback;
						setTimeoutCallback = function () {
							return $loadingFade.modal('hide');
						};
						setTimeout(setTimeoutCallback, 500);
						return fetchAjax;
					});
				};

				return OBNLReinvestmentsList;

			})(Backbone.Model);

			OBNLReinvestmentModel = (function (_super) {

				__extends(OBNLReinvestmentModel, _super);

				OBNLReinvestmentModel.prototype.urlRoot = '/obnlreinvestment/index';

				OBNLReinvestmentModel.prototype.idAttribute = 'Id';

				function OBNLReinvestmentModel() {
					this.url = __bind(this.url, this);
					this.fetchById = __bind(this.fetchById, this); OBNLReinvestmentModel.__super__.constructor.call(this, {
						OBNLReinvestmentNo: '',
						Id: '',
						CreatedAt: '',
						Comment: '',
						CreatedBy: null,
						Attachments: [],
						Materials: [],
						Address: new Backbone.Model({
							City: 'c',
							CItyId: 'i',
							Street: 'd',
							CivicNumber: 's',
							PostalCode: 'a'
						}),
						Client: {
							FirstName: '',
							LastName: '',
							PhoneNumber: '',
							Address: new Backbone.Model({
								City: '',
								CItyId: '',
								Street: '',
								CivicNumber: '',
								PostalCode: ''
							})
						},
						Center: {
							Name: '',
							Url: ''
						}
					});
				}

				OBNLReinvestmentModel.prototype.fetchById = function (id) {
					return this.fetch({
						data: {
							id: id
						}
					});
				};

				OBNLReinvestmentModel.prototype.url = function () {
					if (this.isNew()) return "/obnlreinvestment";
					return "/obnlreinvestment/index/" + this.id;
				};

				return OBNLReinvestmentModel;

			})(Backbone.Model);

			NewOBNLReinvestmentModel = (function (_super) {

				__extends(NewOBNLReinvestmentModel, _super);

				NewOBNLReinvestmentModel.prototype.url = 'obnlreinvestment';

				function NewOBNLReinvestmentModel() {
					this.removeMaterial = __bind(this.removeMaterial, this);
					this.addMaterial = __bind(this.addMaterial, this);
					this.selectClient = __bind(this.selectClient, this); this.Client = ko.observable(null);
					NewOBNLReinvestmentModel.__super__.constructor.call(this, {
						Id: '',
						Comment: '',
						CreatedBy: null,
						ClientId: 0,
						Attachments: [],
						EmployeeName: 0,
						Materials: Array()
					});
				}

				NewOBNLReinvestmentModel.prototype.selectClient = function (client) {
					this.Client(client);
					return this.set({
						ClientId: client.get('Id')
					});
				};

				NewOBNLReinvestmentModel.prototype.addMaterial = function (material) {
					return this.get('Materials').push(material);
				};

				NewOBNLReinvestmentModel.prototype.removeMaterial = function (material) {
					var newArray,
						_this = this;
					newArray = this.get('Materials');
					newArray = newArray.length < 2 ? [] : $.grep(newArray, function (v) {
						return v.Id !== material.Id;
					});
					return this.set({
						'Materials': newArray
					});
				};

				return NewOBNLReinvestmentModel;

			})(Backbone.Model);

			exports.OBNLNumberAutocompleteViewModel = OBNLNumberAutocompleteViewModel;

			exports.OBNLReinvestmentsList = OBNLReinvestmentsList;

			exports.OBNLReinvestmentModel = OBNLReinvestmentModel;

			exports.NewOBNLReinvestmentModel = NewOBNLReinvestmentModel;

		}).call(this);
	}, "obnlreinvestment.form": function (exports, require, module) {
		(function () {
			var ClientViewModel, MaterialViewModel, MaterialsViewModel, ViewModel, client,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

			client = require('client');

			ClientViewModel = (function (_super) {

				__extends(ClientViewModel, _super);

				function ClientViewModel(model) {
					this.showFirstNameLastName = __bind(this.showFirstNameLastName, this);
					this.computeShowContactWarning = __bind(this.computeShowContactWarning, this);
					this.computeShowComment = __bind(this.computeShowComment, this);
					this.computeShowUnverifiedWarning = __bind(this.computeShowUnverifiedWarning, this);
					this.computeFullName = __bind(this.computeFullName, this);
					this.computeShowMaxVisitsReachedWarning = __bind(this.computeShowMaxVisitsReachedWarning, this);
					this.computeShowMaxVisitsWarning = __bind(this.computeShowMaxVisitsWarning, this);
					this.showMaxVisitsWarningAlert = __bind(this.showMaxVisitsWarningAlert, this);
					this.showMaxVisitsReachedAlert = __bind(this.showMaxVisitsReachedAlert, this);
					this.filterIncludedInvoices = __bind(this.filterIncludedInvoices, this);
					this.filterIncludedOBNLReinvestments = __bind(this.filterIncludedOBNLReinvestments, this);
					this.filterExcludedOBNLReinvestments = __bind(this.filterExcludedOBNLReinvestments, this); ClientViewModel.__super__.constructor.call(this, model);
					this.model = model;
					this.categories = new (require('client')).CategoriesModel();
					this.categories.fetch();
					this.fullName = ko.computed(this.computeFullName);
					this.limitsModel = new (require('limits')).Model();
					this.limitsModel.fetchByClientId(this.model.get('Id'));
					this.limits = kb.collectionObservable(this.limitsModel.CurrentLimits);
					this.obnlMaterialsModel = new (require('obnl.materials')).Model();
					this.obnlMaterialsModel.fetchByClientId(this.model.get('Id'));
					this.obnlMaterials = kb.collectionObservable(this.obnlMaterialsModel.CurrentMaterials);
					this.globalSettingsModel = new (require('globalsettings')).Model();
					this.globalSettingsModel.fetch({
						async: false
					});
					this.maxYearlyClientVisitsWarning = ko.observable(this.globalSettingsModel.get('MaxYearlyClientVisitsWarning'));
					this.maxYearlyClientVisits = ko.observable(this.globalSettingsModel.get('MaxYearlyClientVisits'));
					this.invoices = kb.collectionObservable(this.model.Invoices);
					this.includedInvoices = ko.computed(this.filterIncludedInvoices);
					this.obnlReinvestments = kb.collectionObservable(this.model.OBNLReinvestments);
					this.excludedOBNLReinvestments = ko.computed(this.filterExcludedOBNLReinvestments);
					this.includedOBNLReinvestments = ko.computed(this.filterIncludedOBNLReinvestments);
					this.showContactWarning = ko.computed(this.computeShowContactWarning);
					this.showUnverifiedWarning = ko.computed(this.computeShowUnverifiedWarning);
					this.showComment = ko.computed(this.computeShowComment);
					this.showMaxVisitsWarning = ko.computed(this.computeShowMaxVisitsWarning);
					this.showMaxVisitsReachedWarning = ko.computed(this.computeShowMaxVisitsReachedWarning);
				}

				ClientViewModel.prototype.filterExcludedOBNLReinvestments = function () {
					var _this = this;
					return ko.utils.arrayFilter(this.obnlReinvestments(), function (item) {
						return item.IsExcluded();
					});
				};

				ClientViewModel.prototype.filterIncludedOBNLReinvestments = function () {
					var _this = this;
					return ko.utils.arrayFilter(this.obnlReinvestments(), function (item) {
						return !item.IsExcluded();
					});
				};

				ClientViewModel.prototype.filterIncludedInvoices = function () {
					var _this = this;
					return ko.utils.arrayFilter(this.invoices(), function (item) {
						return !item.IsExcluded();
					});
				};

				ClientViewModel.prototype.showMaxVisitsReachedAlert = function () {
					return $('#max-visits-reached-msg-modal').modal('show');
				};

				ClientViewModel.prototype.showMaxVisitsWarningAlert = function () {
					return $('#max-visits-warning-msg-modal').modal('show');
				};

				ClientViewModel.prototype.computeShowMaxVisitsWarning = function () {
					if (this.PersonalVisitsLimit() && this.PersonalVisitsLimit() > 0) {
						return this.obnlReinvestments().length >= (this.PersonalVisitsLimit() - this.maxYearlyClientVisitsWarning() - 1) && this.maxYearlyClientVisitsWarning() > 0 && this.obnlReinvestments().length < this.PersonalVisitsLimit();
					}
					return this.obnlReinvestments().length >= (this.maxYearlyClientVisits() - this.maxYearlyClientVisitsWarning() - 1) && this.maxYearlyClientVisitsWarning() > 0 && this.obnlReinvestments().length < this.maxYearlyClientVisits();
				};

				ClientViewModel.prototype.computeShowMaxVisitsReachedWarning = function () {
					if (this.PersonalVisitsLimit() && this.PersonalVisitsLimit() > 0) {
						return this.obnlReinvestments().length >= this.PersonalVisitsLimit();
					}
					return this.obnlReinvestments().length >= this.maxYearlyClientVisits() && this.maxYearlyClientVisits() > 0;
				};

				ClientViewModel.prototype.computeFullName = function () {
					return this.FirstName() + ' ' + this.LastName();
				};

				ClientViewModel.prototype.computeShowUnverifiedWarning = function () {
					return !this.Verified();
				};

				ClientViewModel.prototype.computeShowComment = function () {
					return this.Comments() !== null && this.Comments() !== '';
				};

				ClientViewModel.prototype.computeShowContactWarning = function () {
					var email, phone;
					email = this.Email();
					phone = this.PhoneNumber();
					return email === null || email.length < 1 || phone === null || phone.length < 0;
				};

				ClientViewModel.prototype.showFirstNameLastName = function () {
					return this.categories.showFirstNameLastName(this.get('Category'));
				};

				return ClientViewModel;

			})(kb.ViewModel);

			MaterialViewModel = (function () {

				function MaterialViewModel(material) {
					this.Name = material.Name;
					this.Unit = material.Unit;
					this.Weight = '';
					this.IsExcluded = material.IsExcluded;
					this.hasFocus = ko.observable(true);
					this.Id = material.Id;
				}

				return MaterialViewModel;

			})();

			MaterialsViewModel = (function () {

				function MaterialsViewModel(model, materialModel) {
					this.sortMaterials = __bind(this.sortMaterials, this);
					this.sortCallback = __bind(this.sortCallback, this);
					this.removeMaterial = __bind(this.removeMaterial, this);
					this.pickMaterial = __bind(this.pickMaterial, this);
					this.showAvailableList = __bind(this.showAvailableList, this);
					this.updateAvailable = __bind(this.updateAvailable, this); this.model = model;
					this.materialsModel = materialModel;
					this.materialsModel.materials.bind('add', this.updateAvailable);
					this.availableMaterials = ko.observableArray([]);
					this.pickedMaterials = ko.observableArray([]);
					this.showAvailable = ko.observable(false);
				}

				MaterialsViewModel.prototype.updateAvailable = function (item) {
					return this.availableMaterials.push(new MaterialViewModel(item.attributes));
				};

				MaterialsViewModel.prototype.showAvailableList = function () {
					return this.showAvailable(true);
				};

				MaterialsViewModel.prototype.pickMaterial = function (m) {
					this.availableMaterials.remove(m);
					this.pickedMaterials.push(m);
					this.model.addMaterial(m);
					return this.sortMaterials();
				};

				MaterialsViewModel.prototype.removeMaterial = function (m) {
					this.model.removeMaterial(m);
					this.availableMaterials.push(m);
					this.pickedMaterials.remove(m);
					return this.sortMaterials();
				};

				MaterialsViewModel.prototype.sortCallback = function (l, r) {
					if (l.Name === r.Name) return 0;
					if (l.Name < r.Name) {
						return -1;
					} else {
						return 1;
					}
				};

				MaterialsViewModel.prototype.sortMaterials = function () {
					this.availableMaterials.sort(this.sortCallback);
					return this.pickedMaterials.sort(this.sortCallback);
				};

				return MaterialsViewModel;

			})();

			ViewModel = (function () {

				function ViewModel(model, materialModel, clientListViewModel, showClients) {
					this.onError = __bind(this.onError, this);
					this.newClient = __bind(this.newClient, this);
					this.editClient = __bind(this.editClient, this);
					this.changeClient = __bind(this.changeClient, this);
					this.onSaved = __bind(this.onSaved, this);
					this.save = __bind(this.save, this);
					this.hidePreview = __bind(this.hidePreview, this);
					this.preview = __bind(this.preview, this);
					this.updatePreview = __bind(this.updatePreview, this);
					this.computePreviewAvailable = __bind(this.computePreviewAvailable, this);
					this.fileUploaded = __bind(this.fileUploaded, this);
					var _this = this;
					this.model = model;
					this.clientList = clientListViewModel;
					this.errors = ko.observableArray();
					this.client = ko.observable(null);
					this.obnlReinvestmentPreview = ko.observable(null);
					model.Client.subscribe(function (c) {
						return _this.client(new ClientViewModel(c));
					});
					this.clientId = kb.observable(model, 'ClientId');
					this.employeeName = kb.observable(model, 'EmployeeName');
					this.comment = kb.observable(model, 'Comment');
					this.materials = new MaterialsViewModel(model, materialModel);
					this.showList = ko.observable(true);
					this.showListPoc = ko.observable(true);
					this.saved = false;
					this.attachments = ko.observableArray([]);
					this.previewAvailable = ko.computed(this.computePreviewAvailable);
					materialModel.fetch();
					this.fileUpload = {
						dataType: 'json',
						done: this.fileUploaded
					};
				}

				ViewModel.prototype.fileUploaded = function (e, f) {
					var att, resp, result;
					resp = f.jqXHR.responseText;
					if (resp === void 0) resp = f.jqXHR.iframe[0].documentElement.innerText;
					result = $.parseJSON(resp);
					att = this.model.get('Attachments');
					att.push(result);
					return this.attachments.push(result);
				};

				ViewModel.prototype.computePreviewAvailable = function () {
					return this.client() !== null;
				};

				ViewModel.prototype.updatePreview = function () {
					var at, material, newPreview, _i, _j, _len, _len2, _ref, _ref2;
					newPreview = {
						OBNLReinvestmentNo: '2013-xxxxxx',
						CreatedAtFormatted: (new Date()).toString('yyyy-MM-dd'),
						Comment: this.comment(),
						CreatedBy: null,
						Client: {
							FirstName: this.client().FirstName(),
							LastName: this.client().LastName(),
							PhoneNumber: this.client().PhoneNumber()
						},
						Address: this.client().Address(),
						Materials: [],
						Attachments: []
					};
					_ref = this.model.attributes.Materials;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						material = _ref[_i];
						newPreview.Materials.push(material);
					}
					_ref2 = this.attachments;
					for (_j = 0, _len2 = _ref2.length; _j < _len2; _j++) {
						at = _ref2[_j];
						newPreview.Attachments.push(at);
					}
					return this.obnlReinvestmentPreview(newPreview);
				};

				ViewModel.prototype.preview = function () {
					this.updatePreview();
					this.errors.removeAll();
					return eco.app.router.navigate('/obnlreinvestment/new/preview');
				};

				ViewModel.prototype.hidePreview = function () {
					return this.obnlReinvestmentPreview(null);
				};

				ViewModel.prototype.save = function () {
					$("#global-loading-fade").modal('show');
					return this.model.save(null, {
						success: this.onSaved,
						error: this.onError
					});
				};

				ViewModel.prototype.onSaved = function () {
					var setTimeoutCallback;
					setTimeoutCallback = function () {
						return $("#global-loading-fade").modal('hide');
					};
					setTimeout(setTimeoutCallback, 500);
					this.saved = true;
					eco.app.notifications.addNotification('msg-invoice-saved-successfully');
					return eco.app.router.navigate('obnlreinvestment/show/' + this.model.get('Id'), {
						trigger: true
					});
				};

				ViewModel.prototype.changeClient = function () {
					this.showListPoc(true);
					return this.showList(true);
				};

				ViewModel.prototype.editClient = function () {
					return eco.app.router.navigate('obnlreinvestment/new/client/edit/' + this.client().Id(), {
						trigger: true
					});
				};

				ViewModel.prototype.newClient = function () {
					return eco.app.router.navigate('obnlreinvestment/new/client/create', {
						trigger: true
					});
				};

				ViewModel.prototype.onError = function (m, errors, data) {
					var item, setTimeoutCallback, _i, _len, _results;
					setTimeoutCallback = function () {
						return $("#global-loading-fade").modal('hide');
					};
					setTimeout(setTimeoutCallback, 500);
					errors = $.parseJSON(errors.responseText);
					this.errors.removeAll();
					_results = [];
					for (_i = 0, _len = errors.length; _i < _len; _i++) {
						item = errors[_i];
						_results.push(this.errors.push(item));
					}
					return _results;
				};

				return ViewModel;

			})();

			exports.ViewModel = ViewModel;

		}).call(this);
	}, "obnlreinvestment.list": function (exports, require, module) {
		(function () {
			var OBNLReinvestmentRowViewModel, ViewModel,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				_this = this;

			OBNLReinvestmentRowViewModel = (function () {

				function OBNLReinvestmentRowViewModel(data) {
					this.undeleted = __bind(this.undeleted, this);
					this.undelete = __bind(this.undelete, this);
					this.showUrl = __bind(this.showUrl, this);
					this.destroy = __bind(this.destroy, this);
					var ca, center, client, res;
					this.model = data;
					this.OBNLReinvestmentNo = data.get('OBNLReinvestmentNo');
					this.Id = data.get('Id');
					client = data.get('Client');
					center = data.get('Center');
					this.CenterUrl = center.Url;
					if (client === void 0 || client === null) {
						this.Client = {
							FirstName: '',
							Category: '',
							LastName: '',
							Address: {
								City: '',
								CivicNumber: '',
								Street: '',
								PostalCode: ''
							}
						};
					} else {
						this.Client = client;
					}
					this.onUndeleted = null;
					ca = data.get('CreatedAt');
					res = ca.match(/\d+/g)[0];
					ca = new Date(res * 1);
					this.CreatedAt = ca.toString('yyyy-MM-dd hh:mm:ss');
				}

				OBNLReinvestmentRowViewModel.prototype.destroy = function () {
					if (!confirm('êtes-vous sûr de vouloir supprimer la facture')) return;
					return this.model.destroy();
				};

				OBNLReinvestmentRowViewModel.prototype.showUrl = function () {
					return '#obnlreinvestment/show/' + this.Id;
				};

				OBNLReinvestmentRowViewModel.prototype.undelete = function () {
					var data;
					data = {
						id: this.Id
					};
					return $.post('/obnlreinvestment/undelete', data, this.undeleted);
				};

				OBNLReinvestmentRowViewModel.prototype.undeleted = function (data) {
					if (this.onUndeleted == null) return;
					return this.onUndeleted(data);
				};

				return OBNLReinvestmentRowViewModel;

			})();

			ViewModel = (function () {

				function ViewModel(model) {
					this.sort = __bind(this.sort, this);
					this.togglePreviousYears = __bind(this.togglePreviousYears, this);
					this.toggleCurrentYear = __bind(this.toggleCurrentYear, this);
					this.search = __bind(this.search, this);
					this.changePage = __bind(this.changePage, this);
					this.load = __bind(this.load, this);
					this.goToPage = __bind(this.goToPage, this);
					this.computeShowDateFields = __bind(this.computeShowDateFields, this);
					var vmItemFactory,
						_this = this;
					this.model = model;
					vmItemFactory = function (data) {
						var item;
						item = new OBNLReinvestmentRowViewModel(data);
						item.onUndeleted = _this.load;
						return item;
					};
					this.items = kb.collectionObservable(this.model.obnlReinvestment, {
						create: vmItemFactory
					});
					this.pageButtons = kb.collectionObservable(this.model.pageButtons);
					this.page = kb.observable(model, 'page');
					this.pageCount = kb.observable(model, 'pageCount');
					this.throttledPage = ko.computed(this.page).extend({
						throttle: 400
					});
					this.pageSize = kb.observable(model, 'pageSize');
					this.term = kb.observable(model, 'filterTerm');
					this.termFrom = kb.observable(model, 'filterTermFrom');
					this.termTo = kb.observable(model, 'filterTermTo');
					this.searchType = kb.observable(model, 'filterType');
					this.sortBy = kb.observable(model, 'sortBy');
					this.sortDir = kb.observable(model, 'sortDir');
					this.searchfocus = ko.observable(true);
					this.deleted = kb.observable(model, 'filterDeleted');
					this.isCurrentYear = kb.observable(model, 'CurrentYear');
					this.centerName = kb.observable(model, 'centerName');
					this.showDateFields = ko.observable(false);
					this.searchType.subscribe(this.computeShowDateFields);
					this.throttledPage.subscribe(this.goToPage);
				}

				ViewModel.prototype.computeShowDateFields = function () {
					var res;
					res = this.searchType() === "OBNLReinvestmentDate";
					return this.showDateFields(res);
				};

				ViewModel.prototype.goToPage = function (val) {
					if (val && val > 1 && val < this.pageCount()) {
						return this.changePage({
							number: val
						});
					}
				};

				ViewModel.prototype.load = function () {
					return this.model.changePage(1);
				};

				ViewModel.prototype.changePage = function (vm) {
					var page;
					page = vm != null ? vm.number : 1;
					return this.model.changePage(page);
				};

				ViewModel.prototype.search = function () {
					return this.model.search();
				};

				ViewModel.prototype.toggleCurrentYear = function () {
					this.isCurrentYear(true);
					return this.load();
				};

				ViewModel.prototype.togglePreviousYears = function () {
					this.isCurrentYear(false);
					return this.load();
				};

				ViewModel.prototype.sort = function (sortBy) {
					if (sortBy !== this.sortBy()) {
						this.sortBy(sortBy);
						this.sortDir('desc');
					} else {
						this.sortDir(this.sortDir() === 'desc' ? 'asc' : 'desc');
					}
					return this.model.changePage(1);
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var model;
				model = new (require('obnlreinvestement')).OBNLReinvestmentsList();
				return new ViewModel(model);
			};

		}).call(this);
	}, "obnlreinvestment.show": function (exports, require, module) {
		(function () {
			var ViewModel, obnlReinvestment,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			obnlReinvestment = require('obnlreinvestment');

			ViewModel = (function (_super) {

				__extends(ViewModel, _super);

				function ViewModel(model) {
					this.computeCreatedAt = __bind(this.computeCreatedAt, this);
					this.load = __bind(this.load, this); ViewModel.__super__.constructor.call(this, model);
					this.model = model;
					this.CreatedAtFormatted = ko.computed(this.computeCreatedAt);
				}

				ViewModel.prototype.load = function (id) {
					return this.model.fetchById(id);
				};

				ViewModel.prototype.computeCreatedAt = function () {
					var ca, res;
					ca = this.CreatedAt();
					res = ca.match(/\d+/g);
					if (res === null || res.length < 1) return '';
					res = res[0];
					ca = new Date(res * 1);
					return ca.toString('yyyy-MM-dd hh:mm:ss');
				};

				return ViewModel;

			})(kb.ViewModel);

			exports.createViewModel = function () {
				return new ViewModel(new obnlReinvestment.OBNLReinvestmentModel());
			};

		}).call(this);
	}, "obnlreinvestment.workflow": function (exports, require, module) {
		(function () {
			var OBNLReinvestmentWorkflow,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; };

			OBNLReinvestmentWorkflow = (function () {

				function OBNLReinvestmentWorkflow(navigate, remoteTemplate) {
					this.clientEdit = __bind(this.clientEdit, this);
					this.clientCreate = __bind(this.clientCreate, this);
					this.clientChange = __bind(this.clientChange, this);
					this.showForm = __bind(this.showForm, this);
					this.showClientList = __bind(this.showClientList, this);
					this.newOBNLReinvestment = __bind(this.newOBNLReinvestment, this);
					this.redirectIfNewOBNLReinvestment = __bind(this.redirectIfNewOBNLReinvestment, this); this.remoteTemplate = remoteTemplate;
					this.navigate = navigate;
					this.formViewModel = null;
					this.obnlReinvestment = null;
				}

				OBNLReinvestmentWorkflow.prototype.redirectIfNewOBNLReinvestment = function (obnl) {
					if (this.formViewModel === null) {
						this.navigate('obnlreinvestment/new');
						true;
					}
					return false;
				};

				OBNLReinvestmentWorkflow.prototype.newOBNLReinvestment = function () {
					var clientListVm, materials,
						_this = this;
					this.obnlReinvestment = new (require('obnlreinvestment')).NewOBNLReinvestmentModel();
					materials = new (require('material')).ListModel();
					materials.filter = function (m) {
						return m.Active;
					};
					clientListVm = this.showClientList();
					clientListVm.reset();
					this.formViewModel = new (require('obnlreinvestment.form')).ViewModel(this.obnlReinvestment, materials, clientListVm, false);
					return this.navigate('obnlreinvestment/new/form');
				};

				OBNLReinvestmentWorkflow.prototype.showClientList = function (setup) {
					var clientListVm,
						_this = this;
					if (setup == null) setup = null;
					clientListVm = new (require('client.list')).createViewModel();
					clientListVm.mode('pick');
					clientListVm.model.set({
						noCommercial: true,
						filterOBNLNumber: '',
						searchType: 'OBNLNumber'
					});
					clientListVm.model.set({
						filterCategory: 'OBNL'
					});
					clientListVm.onNew = function () {
						return _this.navigate('obnlreinvestment/new/client/create');
					};
					clientListVm.onEdit = function (vm) {
						return _this.navigate('obnlreinvestment/new/client/edit/' + vm.Id());
					};
					clientListVm.onPick = function (client) {
						_this.obnlReinvestment.selectClient(client.model());
						_this.formViewModel.showListPoc(false);
						return _this.formViewModel.showList(false);
					};
					if (setup != null) setup(clientListVm);
					return clientListVm;
				};

				OBNLReinvestmentWorkflow.prototype.showForm = function () {
					var _ref;
					if ((_ref = this.formViewModel) != null ? _ref.saved : void 0) {
						this.navigate('obnlreinvestment/new');
					}
					if (this.redirectIfNewOBNLReinvestment()) return;
					this.formViewModel.obnlReinvestmentPreview(null);
					return this.remoteTemplate('obnlreinvestment_newTemplate', this.formViewModel);
				};

				OBNLReinvestmentWorkflow.prototype.clientChange = function () {
					var _this = this;
					if (this.redirectIfNewOBNLReinvestment()) return;
					return this.showClientList(function (cvm) {
						return cvm.onCancel = function () {
							return _this.navigate('obnlreinvestment/new/form');
						};
					});
				};

				OBNLReinvestmentWorkflow.prototype.clientCreate = function () {
					var clientFormVM,
						_this = this;
					if (this.redirectIfNewOBNLReinvestment()) return;
					clientFormVM = new (require('client.form')).createViewModel();
					clientFormVM.onCancel = function () {
						return _this.navigate('obnlreinvestment/new/client/change');
					};
					this.remoteTemplate('client_newTemplate', clientFormVM);
					return clientFormVM.onSaved = function (client) {
						_this.obnlReinvestment.selectClient(client);
						_this.formViewModel.showList(false);
						_this.formViewModel.showListPoc(false);
						_this.navigate('obnlreinvestment/new/form');
						return false;
					};
				};

				OBNLReinvestmentWorkflow.prototype.clientEdit = function (id) {
					var clientFormVM,
						_this = this;
					if (this.redirectIfNewOBNLReinvestment()) return;
					clientFormVM = new (require('client.form')).createViewModel();
					clientFormVM.onCancel = function () {
						return _this.navigate('obnlreinvestment/new/client/change');
					};
					this.remoteTemplate('client_newTemplate', clientFormVM);
					clientFormVM.load(id);
					return clientFormVM.onSaved = function (client) {
						_this.obnlReinvestment.selectClient(client);
						_this.formViewModel.showList(false);
						_this.formViewModel.showListPoc(false);
						_this.navigate('obnlreinvestment/new/form');
						return false;
					};
				};

				return OBNLReinvestmentWorkflow;

			})();

			exports.Workflow = OBNLReinvestmentWorkflow;

		}).call(this);
	}, "reports.journal": function (exports, require, module) {
		(function () {
			var Report, ViewModel, common,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			common = require('common');

			Report = (function (_super) {

				__extends(Report, _super);

				function Report(name) {
					this.fetch = __bind(this.fetch, this);
					this.download = __bind(this.download, this);
					this.serializeArguments = __bind(this.serializeArguments, this);
					this.parse = __bind(this.parse, this); Report.__super__.constructor.call(this, {
						page: 1,
						pageSize: null,
						from: 1..month().ago(),
						material: '',
						to: Date.today(),
						city: 0,
						orderDir: 'Desc',
						orderBy: 'Date',
						processing: false,
						HubId: null,
						pageCount: 1
					});
					this.items = new Backbone.Collection();
					this.pageButtons = new Backbone.Collection();
					this.url = 'reports/' + name;
					this.invoiceCount = 0;
					this.uniqueAddressCount = 0;
					this.totalAmountIncludingTaxes = 0;
				}

				Report.prototype.parse = function (resp) {
					var item, page, pages, _i, _j, _len, _len2, _ref;
					this.items.reset();
					this.invoiceCount = resp.Summary.InvoiceCount;
					this.uniqueAddressCount = resp.Summary.UniqueAddressCount;
					this.totalAmountIncludingTaxes = resp.Summary.TotalAmountIncludingTaxes;
					resp = resp.Report;
					_ref = resp.Items;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						this.items.push(item);
					}
					this.pageButtons.reset();
					pages = common.generatePages(resp.Page, resp.PageCount);
					this.set('pageCount', resp.PageCount);
					for (_j = 0, _len2 = pages.length; _j < _len2; _j++) {
						page = pages[_j];
						this.pageButtons.push(page);
					}
					return this.set({
						processing: false
					});
				};

				Report.prototype.serializeArguments = function (xls) {
					return {
						from: this.get('from').toString('yyyy-MM-dd'),
						to: this.get('to').toString('yyyy-MM-dd'),
						city: this.get('city'),
						page: this.get('page'),
						pageSize: this.get('pageSize'),
						material: this.get('material'),
						xls: xls,
						HubId: this.get('HubId'),
						OrderBy: this.get('orderBy'),
						OrderDir: this.get('orderDir')
					};
				};

				Report.prototype.download = function () {
					return window.location = this.url + "?" + $.param(this.serializeArguments(true));
				};

				Report.prototype.fetch = function () {
					this.set({
						processing: true
					});
					return Report.__super__.fetch.call(this, {
						data: this.serializeArguments(false)
					});
				};

				return Report;

			})(Backbone.Model);

			ViewModel = (function () {

				function ViewModel(model, municipalities, hubs) {
					this.goToPage = __bind(this.goToPage, this);
					this.changePage = __bind(this.changePage, this);
					this.sort = __bind(this.sort, this);
					this.load = __bind(this.load, this);
					this.generateXls = __bind(this.generateXls, this);
					this.generate = __bind(this.generate, this);
					this.computeShowHubs = __bind(this.computeShowHubs, this); this.model = model;
					this.city = kb.observable(model, 'city');
					this.HubId = kb.observable(model, 'HubId');
					this.municipalities = kb.collectionObservable(municipalities.municipalities);
					this.from = kb.observable(model, 'from');
					this.to = kb.observable(model, 'to');
					this.orderBy = kb.observable(model, 'orderBy');
					this.orderDir = kb.observable(model, 'orderDir');
					this.processing = kb.observable(model, 'processing');
					this.material = kb.observable(model, 'material');
					this.hubs = kb.collectionObservable(hubs.hubs);
					this.showHub = ko.computed(this.computeShowHubs);
					this.page = kb.observable(model, 'page');
					this.pageSize = kb.observable(model, 'pageSize');
					this.items = kb.collectionObservable(model.items);
					this.pageButtons = kb.collectionObservable(this.model.pageButtons);
					this.invoiceCount = kb.observable(model, 'invoiceCount');
					this.uniqueAddressCount = kb.observable(model, 'uniqueAddressCount');
					this.totalAmountIncludingTaxes = kb.observable(model, 'totalAmountIncludingTaxes');
					this.pageCount = kb.observable(model, 'pageCount');
					this.throttledPage = ko.computed(this.page).extend({
						throttle: 400
					});
					this.throttledPage.subscribe(this.goToPage);
				}

				ViewModel.prototype.computeShowHubs = function () {
					return this.hubs().length > 0;
				};

				ViewModel.prototype.generate = function () {
					return this.load();
				};

				ViewModel.prototype.generateXls = function () {
					return this.model.download();
				};

				ViewModel.prototype.load = function () {
					this.page(1);
					return this.model.fetch();
				};

				ViewModel.prototype.sort = function (orderBy) {
					if (this.orderBy() !== orderBy) {
						this.orderBy(orderBy);
						this.orderDir('Asc');
					} else {
						this.orderDir(this.orderDir() === 'Asc' ? 'Desc' : 'Asc');
					}
					return this.load();
				};

				ViewModel.prototype.changePage = function (vm) {
					this.page(vm.number);
					return this.model.fetch();
				};

				ViewModel.prototype.goToPage = function (val) {
					val = Number(val);
					if (val && val >= 1 && val <= this.pageCount() && val !== this.page()) {
						return this.changePage({
							number: val
						});
					}
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var hubs, model, municipalities;
				municipalities = new (require('municipality')).ListModel();
				municipalities.filter = function (item) {
					return item.Enabled;
				};
				municipalities.fetch();
				hubs = new (require('hub')).ListModel();
				hubs.fetch();
				model = new Report('journal');
				return new ViewModel(model, municipalities, hubs);
			};

		}).call(this);
	}, "reports.limits": function (exports, require, module) {
		(function () {
			var ItemViewModel, Report, ViewModel, common,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			common = require('common');

			Report = (function (_super) {

				__extends(Report, _super);

				function Report(name) {
					this.fetch = __bind(this.fetch, this);
					this.parse = __bind(this.parse, this); Report.__super__.constructor.call(this, {
						page: 1,
						page: 1,
						pageCount: 1,
						pageSize: null
					});
					this.items = new Backbone.Collection();
					this.pageButtons = new Backbone.Collection();
					this.url = 'reports/limits';
				}

				Report.prototype.parse = function (resp) {
					var item, page, pages, _i, _j, _len, _len2, _ref, _results;
					this.items.reset();
					_ref = resp.Items;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						this.items.push(item);
					}
					this.pageButtons.reset();
					pages = common.generatePages(resp.Page, resp.PageCount);
					this.set('page', resp.Page);
					this.set('pageCount', resp.PageCount);
					_results = [];
					for (_j = 0, _len2 = pages.length; _j < _len2; _j++) {
						page = pages[_j];
						_results.push(this.pageButtons.push(page));
					}
					return _results;
				};

				Report.prototype.fetch = function () {
					return Report.__super__.fetch.call(this, {
						data: {
							page: this.get('page')
						}
					});
				};

				return Report;

			})(Backbone.Model);

			ItemViewModel = (function (_super) {

				__extends(ItemViewModel, _super);

				function ItemViewModel(model) {
					this.fold = __bind(this.fold, this);
					this.expand = __bind(this.expand, this); ItemViewModel.__super__.constructor.call(this, model);
					this.expanded = ko.observable(false);
				}

				ItemViewModel.prototype.expand = function () {
					return this.expanded(true);
				};

				ItemViewModel.prototype.fold = function () {
					return this.expanded(false);
				};

				return ItemViewModel;

			})(Knockback.ViewModel);

			ViewModel = (function () {

				function ViewModel(model) {
					this.changePage = __bind(this.changePage, this);
					this.load = __bind(this.load, this);
					this.goToPage = __bind(this.goToPage, this); this.model = model;
					this.page = kb.observable(model, 'page');
					this.pageCount = kb.observable(model, 'pageCount');
					this.throttledPage = ko.computed(this.page).extend({
						throttle: 400
					});
					this.pageSize = kb.observable(model, 'pageSize');
					this.items = kb.collectionObservable(model.items, {
						view_model: ItemViewModel
					});
					this.pageButtons = kb.collectionObservable(this.model.pageButtons);
					this.throttledPage.subscribe(this.goToPage);
				}

				ViewModel.prototype.goToPage = function (val) {
					if (val && val > 1 && val < this.pageCount()) {
						return this.changePage({
							number: val
						});
					}
				};

				ViewModel.prototype.load = function () {
					this.page(1);
					return this.model.fetch();
				};

				ViewModel.prototype.changePage = function (vm) {
					if (!vm.number) vm.number = 1;
					this.page(vm.number);
					return this.model.fetch();
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var model;
				model = new Report();
				return new ViewModel(model);
			};

		}).call(this);
	}, "reports.materials": function (exports, require, module) {
		(function () {
			var Report, ViewModel, common,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			common = require('common');

			Report = (function (_super) {

				__extends(Report, _super);

				function Report(name) {
					this.fetch = __bind(this.fetch, this);
					this.download = __bind(this.download, this);
					this.serializeArguments = __bind(this.serializeArguments, this);
					this.parse = __bind(this.parse, this); Report.__super__.constructor.call(this, {
						from: 1..month().ago(),
						to: Date.today(),
						sortBy: 'Name',
						sortDir: 'Asc'
					});
					this.items = new Backbone.Collection();
					this.pageButtons = new Backbone.Collection();
					this.url = 'reports/' + name;
					this.InvoiceCount = 0;
					this.UniqueAddressCount = 0;
				}

				Report.prototype.parse = function (resp) {
					var item, page, pages, _i, _j, _len, _len2, _results;
					this.items.reset();
					for (_i = 0, _len = resp.length; _i < _len; _i++) {
						item = resp[_i];
						this.items.push(item);
					}
					this.pageButtons.reset();
					pages = common.generatePages(resp.Page, resp.PageCount);
					_results = [];
					for (_j = 0, _len2 = pages.length; _j < _len2; _j++) {
						page = pages[_j];
						_results.push(this.pageButtons.push(page));
					}
					return _results;
				};

				Report.prototype.serializeArguments = function (xls) {
					return {
						from: this.get('from').toString('yyyy-MM-dd'),
						to: this.get('to').toString('yyyy-MM-dd'),
						Xls: xls,
						sortBy: this.get('sortBy'),
						sortDir: this.get('sortDir')
					};
				};

				Report.prototype.download = function () {
					return window.location = this.url + "?" + $.param(this.serializeArguments(true));
				};

				Report.prototype.fetch = function () {
					return Report.__super__.fetch.call(this, {
						data: this.serializeArguments(false)
					});
				};

				return Report;

			})(Backbone.Model);

			ViewModel = (function () {

				function ViewModel(model) {
					this.load = __bind(this.load, this);
					this.generateXls = __bind(this.generateXls, this);
					this.sort = __bind(this.sort, this);
					this.generate = __bind(this.generate, this); this.model = model;
					this.from = kb.observable(model, 'from');
					this.to = kb.observable(model, 'to');
					this.items = kb.collectionObservable(model.items);
					this.sortBy = kb.observable(model, 'sortBy');
					this.sortDir = kb.observable(model, 'sortDir');
					this.pageButtons = kb.collectionObservable(this.model.pageButtons);
				}

				ViewModel.prototype.generate = function () {
					return this.load();
				};

				ViewModel.prototype.sort = function (sortBy) {
					if (this.sortBy() !== sortBy) {
						this.sortBy(sortBy);
						this.sortDir('Asc');
					} else {
						this.sortDir(this.sortDir() === 'Asc' ? 'Desc' : 'Asc');
					}
					return this.load();
				};

				ViewModel.prototype.generateXls = function () {
					return this.model.download();
				};

				ViewModel.prototype.load = function () {
					return this.model.fetch();
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var model;
				model = new Report('materials');
				return new ViewModel(model);
			};

		}).call(this);
	}, "reports.materialsbyaddress": function (exports, require, module) {
		(function () {
			var ItemViewModel, Report, ViewModel, client, common, hub,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			common = require('common');

			client = require('client');

			hub = require('hub');

			Report = (function (_super) {

				__extends(Report, _super);

				function Report(name) {
					this.fetch = __bind(this.fetch, this);
					this.download = __bind(this.download, this);
					this.serializeArguments = __bind(this.serializeArguments, this);
					this.parse = __bind(this.parse, this); Report.__super__.constructor.call(this, {
						centerName: '',
						filterFirstName: '',
						filterLastName: '',
						filterStreet: '',
						filterCivicNumber: '',
						filterCitizenCard: '',
						filterPostalCode: '',
						filterLastChange:'',
						searchType: 'Address',
						searchTerm: '',
						page: 1,
						pageCount: 1,
						pageSize: null,
						sortBy: 'Name',
						sortIndex: 0,
						sortDir: 'Asc',
						fromDate: Date.today().add(-30).days(),
						toDate: Date.today(),
						personalVisitsLimitHigherThenGlobalOnly: false,
						allClients: false
					});
					this.items = new Backbone.Collection();
					this.pageButtons = new Backbone.Collection();
					this.url = 'reports/' + name;
				}

				Report.prototype.parse = function (resp) {
					var item, page, pages, _i, _j, _len, _len2, _ref, _results;
					this.items.reset();
					_ref = resp.Items;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						this.items.push(item);
					}
					this.pageButtons.reset();
					pages = common.generatePages(resp.Page, resp.PageCount);
					this.set('page', resp.Page);
					this.set('pageCount', resp.PageCount);
					_results = [];
					for (_j = 0, _len2 = pages.length; _j < _len2; _j++) {
						page = pages[_j];
						_results.push(this.pageButtons.push(page));
					}
					return _results;
				};

				Report.prototype.serializeArguments = function (xls) {
					return {
						filterFirstName: this.get('filterFirstName'),
						filterLastName: this.get('filterLastName'),
						filterCitizenCard: this.get('filterCitizenCard'),
						filterLastChange: this.get('filterLastChange'),
						filterStreet: this.get('filterStreet'),
						filterCivicNumber: this.get('filterCivicNumber'),
						filterPostalCode: this.get('filterPostalCode'),
						searchTerm: this.get('searchTerm'),
						searchType: this.get('searchType'),
						page: this.get('page'),
						pageSize: this.get('pageSize'),
						Xls: xls,
						sortBy: this.get('sortBy'),
						sortIndex: this.get('sortIndex'),
						sortDir: this.get('sortDir'),
						fromDate: this.get('fromDate') ? this.get('fromDate').toString('yyyy-MM-dd') : null,
						toDate: this.get('toDate') ? this.get('toDate').toString('yyyy-MM-dd') : null,
						centerName: this.get('hub'),
						personalVisitsLimitHigherThenGlobalOnly: this.get('personalVisitsLimitHigherThenGlobalOnly'),
						allClients: this.get('allClients')
					};
				};

				Report.prototype.download = function () {
					return window.location = this.url + "?" + $.param(this.serializeArguments(true));
				};

				Report.prototype.fetch = function () {
					var $loadingFade, fetchAjax, serializedData,
						_this = this;
					serializedData = this.serializeArguments(false);
					$loadingFade = $("#global-loading-fade");
					$loadingFade.modal('show');
					fetchAjax = Report.__super__.fetch.call(this, {
						data: serializedData
					});
					return fetchAjax.complete(function () {
						var setTimeoutCallback;
						setTimeoutCallback = function () {
							return $loadingFade.modal('hide');
						};
						setTimeout(setTimeoutCallback, 500);
						return fetchAjax;
					});
				};

				return Report;

			})(Backbone.Model);

			ItemViewModel = (function (_super) {

				__extends(ItemViewModel, _super);

				function ItemViewModel(model) {
					this.fold = __bind(this.fold, this);
					this.expand = __bind(this.expand, this); ItemViewModel.__super__.constructor.call(this, model);
					this.expanded = ko.observable(false);
				}

				ItemViewModel.prototype.expand = function () {
					return this.expanded(true);
				};

				ItemViewModel.prototype.fold = function () {
					return this.expanded(false);
				};

				return ItemViewModel;

			})(Knockback.ViewModel);

			ViewModel = (function () {

				function ViewModel(model) {
					this.changePage = __bind(this.changePage, this);
					this.generateXls = __bind(this.generateXls, this);
					this.sort = __bind(this.sort, this);
					this.search = __bind(this.search, this);
					this.onHubsChange = __bind(this.onHubsChange, this);
					this.load = __bind(this.load, this);
					this.goToPage = __bind(this.goToPage, this);
					this.computeShowAddress = __bind(this.computeShowAddress, this);
					this.computeShowName = __bind(this.computeShowName, this); this.model = model;
					this.page = kb.observable(model, 'page');
					this.pageCount = kb.observable(model, 'pageCount');
					this.throttledPage = ko.computed(this.page).extend({
						throttle: 400
					});
					this.pageSize = kb.observable(model, 'pageSize');
					this.items = kb.collectionObservable(model.items, {
						view_model: ItemViewModel
					});
					this.sortBy = kb.observable(model, 'sortBy');
					this.sortIndex = kb.observable(model, 'sortIndex');
					this.sortDir = kb.observable(model, 'sortDir');
					this.searchType = kb.observable(model, 'searchType');
					this.firstName = kb.observable(model, 'filterFirstName');
					this.lastName = kb.observable(model, 'filterLastName');
					this.citizenCard = kb.observable(model, 'filterCitizenCard');
					this.lastChange = kb.observable(model, 'filterLastChange');
					this.street = kb.observable(model, 'filterStreet');
					this.civicNumber = kb.observable(model, 'filterCivicNumber');
					this.postalCode = kb.observable(model, 'filterPostalCode');
					this.hub = kb.observable(model, 'hub');
					this.hubsModel = new hub.ListModel();
					this.hubsModel.on("change", this.onHubsChange);
					this.hubs = ko.observableArray([]);
					this.hubsModel.fetch();
					this.personalVisitsLimitHigherThenGlobalOnly = kb.observable(model, 'personalVisitsLimitHigherThenGlobalOnly');
					this.allClients = kb.observable(model, 'allClients');
					this.lastNameAutocomplete = new client.ClientLastNameAutocompleteViewModel(this.lastName);
					this.firstNameAutocomplete = new client.ClientFirstNameAutocompleteViewModel(this.firstName);
					this.streetNameAutocomplete = new client.ClientStreetNameAutocompleteViewModel(this.civicNumber, this.street);
					this.civicNumberAutocomplete = new client.ClientCivicNumberAutocompleteViewModel(this.civicNumber, this.street);
					this.civicCardAutocomplete = new client.ClientCivicCardAutocompleteViewModel(this.civicCard);
					this.postalCodeAutocomplete = new client.ClientPostalCodeAutocompleteViewModel(this.postalCode);
					this.searchTerm = kb.observable(model, 'searchTerm');
					this.fromDate = kb.observable(model, 'fromDate');
					this.toDate = kb.observable(model, 'toDate');
					this.searchfocus = ko.observable(true);
					this.pageButtons = kb.collectionObservable(this.model.pageButtons);
					this.showName = ko.observable(true);
					this.showAddress = ko.observable(false);
					this.computeShowName();
					this.computeShowAddress();
					this.searchType.subscribe(this.computeShowName);
					this.searchType.subscribe(this.computeShowAddress);
					this.pageButtons = kb.collectionObservable(this.model.pageButtons);
					this.showDateFields = ko.observable(false);
					this.throttledPage.subscribe(this.goToPage);
					this.globalSettingsModel = new (require('globalsettings')).Model();
					this.globalSettingsModel.fetch({
						async: false
					});
					this.maxYearlyClientVisits = ko.observable(this.globalSettingsModel.get('MaxYearlyClientVisits'));
				}

				ViewModel.prototype.computeShowName = function () {
					return this.showName(this.searchType().toLowerCase() === "name");
				};

				ViewModel.prototype.computeShowAddress = function () {
					return this.showAddress(this.searchType().toLowerCase() === "address");
				};

				ViewModel.prototype.goToPage = function (val) {
					if (val && val > 1 && val < this.pageCount()) {
						return this.changePage({
							number: val
						});
					}
				};

				ViewModel.prototype.load = function () {
					return this.model.fetch();
				};

				ViewModel.prototype.onHubsChange = function (items) {
					var hubs, i, name, _ref;
					hubs = ["Tous"];
					for (i = 0, _ref = items.hubList.models.length; 0 <= _ref ? i < _ref : i > _ref; 0 <= _ref ? i++ : i--) {
						name = items.hubList.models[i].get("Name");
						hubs.push(name);
					}
					return this.hubs(hubs);
				};

				ViewModel.prototype.search = function () {
					this.page(1);
					return this.model.fetch();
				};

				ViewModel.prototype.sort = function (sortBy, sortIndex) {
					sortIndex = (sortIndex != null ? sortIndex : 0);
					if (this.sortBy() !== sortBy || this.sortIndex() !== sortIndex) {
						this.sortBy(sortBy);
						this.sortIndex(sortIndex);
						this.sortDir('Asc');
					} else {
						this.sortDir(this.sortDir() === 'Asc' ? 'Desc' : 'Asc');
					}
					return this.load();
				};

				ViewModel.prototype.generateXls = function () {
					return this.model.download();
				};

				ViewModel.prototype.changePage = function (vm) {
					this.page(vm.number);
					return this.model.fetch();
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var model;
				model = new Report("materialsbyaddress");
				return new ViewModel(model);
			};

		}).call(this);
	}, "reports.obnlglobal": function (exports, require, module) {
		(function () {
			var ItemViewModel, Report, ViewModel, client, common,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			common = require('common');

			client = require('client');

			Report = (function (_super) {

				__extends(Report, _super);

				function Report(name) {
					this.fetch = __bind(this.fetch, this);
					this.download = __bind(this.download, this);
					this.serializeArguments = __bind(this.serializeArguments, this);
					this.parse = __bind(this.parse, this); Report.__super__.constructor.call(this, {
						centerName: '',
						OBNLNumber: '',
						page: 1,
						pageCount: 1,
						pageSize: null,
						sortBy: 'Name',
						sortIndex: 0,
						sortDir: 'Asc',
						fromDate: Date.today().set({
							month: 0,
							day: 1
						}),
						toDate: Date.today(),
						allClients: false
					});
					this.items = new Backbone.Collection();
					this.pageButtons = new Backbone.Collection();
					this.url = 'reports/' + name;
				}

				Report.prototype.parse = function (resp) {
					var item, page, pages, _i, _j, _len, _len2, _ref, _results;
					this.items.reset();
					_ref = resp;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						this.items.push(item);
					}
					this.pageButtons.reset();
					pages = common.generatePages(1, 1);
					this.set('page', 1);
					this.set('pageCount', 1);
					_results = [];
					for (_j = 0, _len2 = pages.length; _j < _len2; _j++) {
						page = pages[_j];
						_results.push(this.pageButtons.push(page));
					}
					return _results;
				};

				Report.prototype.serializeArguments = function (xls) {
					return {
						page: this.get('page'),
						pageSize: this.get('pageSize'),
						Xls: xls,
						sortBy: this.get('sortBy'),
						sortIndex: this.get('sortIndex'),
						sortDir: this.get('sortDir'),
						fromDate: this.get('fromDate') ? this.get('fromDate').toString('yyyy-MM-dd') : null,
						toDate: this.get('toDate') ? this.get('toDate').toString('yyyy-MM-dd') : null,
						centerName: this.get('centerName'),
						OBNLNumber: this.get('OBNLNumber'),
						allClients: this.get('allClients')
					};
				};

				Report.prototype.download = function () {
					return window.location = this.url + "?" + $.param(this.serializeArguments(true));
				};

				Report.prototype.fetch = function () {
					var $loadingFade, fetchAjax, serializedData,
						_this = this;
					serializedData = this.serializeArguments(false);
					$loadingFade = $("#global-loading-fade");
					$loadingFade.modal('show');
					fetchAjax = Report.__super__.fetch.call(this, {
						data: serializedData
					});
					return fetchAjax.complete(function () {
						var setTimeoutCallback;
						setTimeoutCallback = function () {
							return $loadingFade.modal('hide');
						};
						setTimeout(setTimeoutCallback, 500);
						return fetchAjax;
					});
				};

				return Report;

			})(Backbone.Model);

			ItemViewModel = (function (_super) {

				__extends(ItemViewModel, _super);

				function ItemViewModel(model) {
					this.fold = __bind(this.fold, this);
					this.expand = __bind(this.expand, this); ItemViewModel.__super__.constructor.call(this, model);
					this.expanded = ko.observable(false);
				}

				ItemViewModel.prototype.expand = function () {
					return this.expanded(true);
				};

				ItemViewModel.prototype.fold = function () {
					return this.expanded(false);
				};

				return ItemViewModel;

			})(Knockback.ViewModel);

			ViewModel = (function () {

				function ViewModel(model) {
					this.changePage = __bind(this.changePage, this);
					this.generateXls = __bind(this.generateXls, this);
					this.sort = __bind(this.sort, this);
					this.search = __bind(this.search, this);
					this.load = __bind(this.load, this);
					this.goToPage = __bind(this.goToPage, this); this.model = model;
					this.page = kb.observable(model, 'page');
					this.pageCount = kb.observable(model, 'pageCount');
					this.throttledPage = ko.computed(this.page).extend({
						throttle: 400
					});
					this.pageSize = kb.observable(model, 'pageSize');
					this.items = kb.collectionObservable(model.items, {
						view_model: ItemViewModel
					});
					this.sortBy = kb.observable(model, 'sortBy');
					this.sortIndex = kb.observable(model, 'sortIndex');
					this.sortDir = kb.observable(model, 'sortDir');
					this.centerName = kb.observable(model, 'centerName');
					this.OBNLNumber = kb.observable(model, 'OBNLNumber');
					this.obnlNumberAutocomplete = new (require('obnlreinvestment')).OBNLNumberAutocompleteViewModel(this.OBNLNumber);
					this.allClients = kb.observable(model, 'allClients');
					this.fromDate = kb.observable(model, 'fromDate');
					this.toDate = kb.observable(model, 'toDate');
					this.searchfocus = ko.observable(true);
					this.pageButtons = kb.collectionObservable(this.model.pageButtons);
					this.pageButtons = kb.collectionObservable(this.model.pageButtons);
					this.showDateFields = ko.observable(false);
					this.throttledPage.subscribe(this.goToPage);
					this.globalSettingsModel = new (require('globalsettings')).Model();
					this.globalSettingsModel.fetch({
						async: false
					});
				}

				ViewModel.prototype.goToPage = function (val) {
					if (val && val > 1 && val < this.pageCount()) {
						return this.changePage({
							number: val
						});
					}
				};

				ViewModel.prototype.load = function () {
					return this.model.fetch();
				};

				ViewModel.prototype.search = function () {
					this.page(1);
					return this.model.fetch();
				};

				ViewModel.prototype.sort = function (sortBy, sortIndex) {
					sortIndex = (sortIndex != null ? sortIndex : 0);
					if (this.sortBy() !== sortBy || this.sortIndex() !== sortIndex) {
						this.sortBy(sortBy);
						this.sortIndex(sortIndex);
						this.sortDir('Asc');
					} else {
						this.sortDir(this.sortDir() === 'Asc' ? 'Desc' : 'Asc');
					}
					return this.load();
				};

				ViewModel.prototype.generateXls = function () {
					return this.model.download();
				};

				ViewModel.prototype.changePage = function (vm) {
					this.page(vm.number);
					return this.model.fetch();
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var model;
				model = new Report("obnlglobal");
				return new ViewModel(model);
			};

		}).call(this);
	}, "reports.obnltotal": function (exports, require, module) {
		(function () {
			var ItemViewModel, Report, ViewModel, client, common,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			common = require('common');

			client = require('client');

			Report = (function (_super) {

				__extends(Report, _super);

				function Report(name) {
					this.fetch = __bind(this.fetch, this);
					this.download = __bind(this.download, this);
					this.serializeArguments = __bind(this.serializeArguments, this);
					this.parse = __bind(this.parse, this); Report.__super__.constructor.call(this, {
						centerName: '',
						OBNLNumber: '',
						filterFirstName: '',
						filterLastName: '',
						filterCitizenCard: '',
						filterLastChange:'',
						filterStreet: '',
						filterCivicNumber: '',
						filterPostalCode: '',
						searchType: 'Address',
						searchTerm: '',
						page: 1,
						pageCount: 1,
						pageSize: null,
						sortBy: 'Name',
						sortIndex: 0,
						sortDir: 'Asc',
						fromDate: Date.today().set({
							month: 0,
							day: 1
						}),
						toDate: Date.today(),
						personalVisitsLimitHigherThenGlobalOnly: false,
						allClients: false
					});
					this.items = new Backbone.Collection();
					this.pageButtons = new Backbone.Collection();
					this.url = 'reports/' + name;
				}

				Report.prototype.parse = function (resp) {
					var item, page, pages, _i, _j, _len, _len2, _ref, _results;
					this.items.reset();
					_ref = resp;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						this.items.push(item);
					}
					this.pageButtons.reset();
					pages = common.generatePages(1, 1);
					this.set('page', 1);
					this.set('pageCount', 1);
					_results = [];
					for (_j = 0, _len2 = pages.length; _j < _len2; _j++) {
						page = pages[_j];
						_results.push(this.pageButtons.push(page));
					}
					return _results;
				};

				Report.prototype.serializeArguments = function (xls) {
					return {
						filterFirstName: this.get('filterFirstName'),
						filterLastChange: this.get('filterLastChange'),
						filterLastName: this.get('filterLastName'),
						filterStreet: this.get('filterStreet'),
						filterCivicNumber: this.get('filterCivicNumber'),
						filterPostalCode: this.get('filterPostalCode'),
						searchTerm: this.get('searchTerm'),
						searchType: this.get('searchType'),
						page: this.get('page'),
						pageSize: this.get('pageSize'),
						Xls: xls,
						sortBy: this.get('sortBy'),
						sortIndex: this.get('sortIndex'),
						sortDir: this.get('sortDir'),
						fromDate: this.get('fromDate') ? this.get('fromDate').toString('yyyy-MM-dd') : null,
						toDate: this.get('toDate') ? this.get('toDate').toString('yyyy-MM-dd') : null,
						centerName: this.get('centerName'),
						OBNLNumber: this.get('OBNLNumber'),
						personalVisitsLimitHigherThenGlobalOnly: this.get('personalVisitsLimitHigherThenGlobalOnly'),
						allClients: this.get('allClients')
					};
				};

				Report.prototype.download = function () {
					return window.location = this.url + "?" + $.param(this.serializeArguments(true));
				};

				Report.prototype.fetch = function () {
					var $loadingFade, fetchAjax, serializedData,
						_this = this;
					serializedData = this.serializeArguments(false);
					$loadingFade = $("#global-loading-fade");
					$loadingFade.modal('show');
					fetchAjax = Report.__super__.fetch.call(this, {
						data: serializedData
					});
					return fetchAjax.complete(function () {
						var setTimeoutCallback;
						setTimeoutCallback = function () {
							return $loadingFade.modal('hide');
						};
						setTimeout(setTimeoutCallback, 500);
						return fetchAjax;
					});
				};

				return Report;

			})(Backbone.Model);

			ItemViewModel = (function (_super) {

				__extends(ItemViewModel, _super);

				function ItemViewModel(model) {
					this.fold = __bind(this.fold, this);
					this.expand = __bind(this.expand, this); ItemViewModel.__super__.constructor.call(this, model);
					this.expanded = ko.observable(false);
				}

				ItemViewModel.prototype.expand = function () {
					return this.expanded(true);
				};

				ItemViewModel.prototype.fold = function () {
					return this.expanded(false);
				};

				return ItemViewModel;

			})(Knockback.ViewModel);

			ViewModel = (function () {

				function ViewModel(model) {
					this.changePage = __bind(this.changePage, this);
					this.generateXls = __bind(this.generateXls, this);
					this.sort = __bind(this.sort, this);
					this.search = __bind(this.search, this);
					this.load = __bind(this.load, this);
					this.goToPage = __bind(this.goToPage, this);
					this.computeShowAddress = __bind(this.computeShowAddress, this);
					this.computeShowName = __bind(this.computeShowName, this);
					var _this = this;
					this.model = model;
					this.page = kb.observable(model, 'page');
					this.pageCount = kb.observable(model, 'pageCount');
					this.throttledPage = ko.computed(this.page).extend({
						throttle: 400
					});
					this.pageSize = kb.observable(model, 'pageSize');
					this.items = kb.collectionObservable(model.items, {
						view_model: ItemViewModel
					});
					this.sortBy = kb.observable(model, 'sortBy');
					this.sortIndex = kb.observable(model, 'sortIndex');
					this.sortDir = kb.observable(model, 'sortDir');
					this.searchType = kb.observable(model, 'searchType');
					this.firstName = kb.observable(model, 'filterFirstName');
					this.lastName = kb.observable(model, 'filterLastName');
					this.citizenCard = kb.observable(model, 'filterCitizenCard');
					this.lastChange = kb.observable(model, 'filterLastChange');
					this.street = kb.observable(model, 'filterStreet');
					this.civicNumber = kb.observable(model, 'filterCivicNumber');
					this.postalCode = kb.observable(model, 'filterPostalCode');
					this.centerName = kb.observable(model, 'centerName');
					this.OBNLNumber = kb.observable(model, 'OBNLNumber');
					this.obnlNumberAutocomplete = new (require('obnlreinvestment')).OBNLNumberAutocompleteViewModel(this.OBNLNumber);
					this.personalVisitsLimitHigherThenGlobalOnly = kb.observable(model, 'personalVisitsLimitHigherThenGlobalOnly');
					this.allClients = kb.observable(model, 'allClients');
					this.lastNameAutocomplete = new client.ClientLastNameAutocompleteViewModel(this.lastName);
					this.firstNameAutocomplete = new client.ClientFirstNameAutocompleteViewModel(this.firstName);
					this.streetNameAutocomplete = new client.ClientStreetNameAutocompleteViewModel(this.civicNumber, this.street);
					this.civicNumberAutocomplete = new client.ClientCivicNumberAutocompleteViewModel(this.civicNumber, this.street);
					this.postalCodeAutocomplete = new client.ClientPostalCodeAutocompleteViewModel(this.postalCode);
					this.civicCardAutocomplete = new client.ClientCivicCardAutocompleteViewModel(this.civicCard);
					this.totalReinvestments = ko.computed(function () {
						return _this.items().reduce((function (x, s) {
							return x + s.Invoices().length;
						}), 0);
					});
					this.totalWeight = ko.computed(function () {
						return _this.items().reduce((function (x, s) {
							return x + s.TotalWeight();
						}), 0);
					});
					this.totalVisits = ko.computed(function () {
						return _this.items().reduce((function (x, s) {
							return x + s.TotalVisits();
						}), 0);
					});
					this.searchTerm = kb.observable(model, 'searchTerm');
					this.fromDate = kb.observable(model, 'fromDate');
					this.toDate = kb.observable(model, 'toDate');
					this.searchfocus = ko.observable(true);
					this.pageButtons = kb.collectionObservable(this.model.pageButtons);
					this.showName = ko.observable(true);
					this.showAddress = ko.observable(false);
					this.computeShowName();
					this.computeShowAddress();
					this.searchType.subscribe(this.computeShowName);
					this.searchType.subscribe(this.computeShowAddress);
					this.pageButtons = kb.collectionObservable(this.model.pageButtons);
					this.showDateFields = ko.observable(false);
					this.throttledPage.subscribe(this.goToPage);
					this.globalSettingsModel = new (require('globalsettings')).Model();
					this.globalSettingsModel.fetch({
						async: false
					});
					this.maxYearlyClientVisits = ko.observable(this.globalSettingsModel.get('MaxYearlyClientVisits'));
				}

				ViewModel.prototype.computeShowName = function () {
					return this.showName(this.searchType().toLowerCase() === "name");
				};

				ViewModel.prototype.computeShowAddress = function () {
					return this.showAddress(this.searchType().toLowerCase() === "address");
				};

				ViewModel.prototype.goToPage = function (val) {
					if (val && val > 1 && val < this.pageCount()) {
						return this.changePage({
							number: val
						});
					}
				};

				ViewModel.prototype.load = function () {
					return this.model.fetch();
				};

				ViewModel.prototype.search = function () {
					this.page(1);
					return this.model.fetch();
				};

				ViewModel.prototype.sort = function (sortBy, sortIndex) {
					sortIndex = (sortIndex != null ? sortIndex : 0);
					if (this.sortBy() !== sortBy || this.sortIndex() !== sortIndex) {
						this.sortBy(sortBy);
						this.sortIndex(sortIndex);
						this.sortDir('Asc');
					} else {
						this.sortDir(this.sortDir() === 'Asc' ? 'Desc' : 'Asc');
					}
					return this.load();
				};

				ViewModel.prototype.generateXls = function () {
					return this.model.download();
				};

				ViewModel.prototype.changePage = function (vm) {
					this.page(vm.number);
					return this.model.fetch();
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var model;
				model = new Report("obnltotal");
				return new ViewModel(model);
			};

		}).call(this);
	}, "reports.visitslimits": function (exports, require, module) {
		(function () {
			var ItemViewModel, Report, ViewModel, client, common,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			common = require('common');

			client = require('client');

			Report = (function (_super) {

				__extends(Report, _super);

				function Report(name) {
					this.fetch = __bind(this.fetch, this);
					this.parse = __bind(this.parse, this); Report.__super__.constructor.call(this, {
						page: 1,
						pageCount: 1,
						pageSize: null
					});
					this.items = new Backbone.Collection();
					this.pageButtons = new Backbone.Collection();
					this.url = 'reports/visitslimits';
				}

				Report.prototype.parse = function (resp) {
					var invoice, item, page, pages, _i, _j, _k, _len, _len2, _len3, _ref, _ref2, _results;
					this.items.reset();
					_ref = resp.Items;
					for (_i = 0, _len = _ref.length; _i < _len; _i++) {
						item = _ref[_i];
						item.$parent = this;
						item.includedInvoicesCount = 0;
						_ref2 = item.Invoices;
						for (_j = 0, _len2 = _ref2.length; _j < _len2; _j++) {
							invoice = _ref2[_j];
							if (!invoice.IsExcluded) item.includedInvoicesCount++;
						}
						this.items.push(item);
					}
					this.pageButtons.reset();
					pages = common.generatePages(resp.Page, resp.PageCount);
					this.set('page', resp.Page);
					this.set('pageCount', resp.PageCount);
					_results = [];
					for (_k = 0, _len3 = pages.length; _k < _len3; _k++) {
						page = pages[_k];
						_results.push(this.pageButtons.push(page));
					}
					return _results;
				};

				Report.prototype.fetch = function () {
					return Report.__super__.fetch.call(this, {
						data: {
							page: this.get('page')
						}
					});
				};

				return Report;

			})(Backbone.Model);

			ItemViewModel = (function (_super) {

				__extends(ItemViewModel, _super);

				function ItemViewModel(model) {
					this.saveEditChanges = __bind(this.saveEditChanges, this);
					this.hideEditInput = __bind(this.hideEditInput, this);
					this.showEditInput = __bind(this.showEditInput, this);
					this.fold = __bind(this.fold, this);
					this.expand = __bind(this.expand, this); ItemViewModel.__super__.constructor.call(this, model);
					this.expanded = ko.observable(false);
					this.edited = ko.observable(false);
					this.newPersVisLimit = ko.observable();
					this.newPersVisLimit(this.model().get('Client').PersonalVisitsLimit || 0);
				}

				ItemViewModel.prototype.expand = function () {
					return this.expanded(true);
				};

				ItemViewModel.prototype.fold = function () {
					return this.expanded(false);
				};

				ItemViewModel.prototype.showEditInput = function () {
					return this.edited(true);
				};

				ItemViewModel.prototype.hideEditInput = function () {
					this.edited(false);
					return this.newPersVisLimit(this.model().get('Client').PersonalVisitsLimit || 0);
				};

				ItemViewModel.prototype.saveEditChanges = function () {
					var $loadingFade, curClient,
						_this = this;
					if (!this.newPersVisLimit() && this.newPersVisLimit() !== 0) return;
					curClient = new client.Model(this.model().get('Client'));
					curClient.set({
						'PersonalVisitsLimit': this.newPersVisLimit(),
						'UpdateOnlyPersonalVisitsLimit': true
					});
					$loadingFade = $("#global-loading-fade");
					$loadingFade.modal('show');
					return curClient.save().complete(function () {
						_this.$parent().model().set({
							page: 1
						});
						return _this.$parent().model().fetch().complete(function () {
							var setTimeoutCallback;
							setTimeoutCallback = function () {
								return $loadingFade.modal('hide');
							};
							return setTimeout(setTimeoutCallback, 500);
						});
					});
				};

				return ItemViewModel;

			})(Knockback.ViewModel);

			ViewModel = (function () {

				function ViewModel(model) {
					this.changePage = __bind(this.changePage, this);
					this.load = __bind(this.load, this);
					this.goToPage = __bind(this.goToPage, this); this.model = model;
					this.page = kb.observable(model, 'page');
					this.pageCount = kb.observable(model, 'pageCount');
					this.throttledPage = ko.computed(this.page).extend({
						throttle: 400
					});
					this.pageSize = kb.observable(model, 'pageSize');
					this.items = kb.collectionObservable(model.items, {
						view_model: ItemViewModel
					});
					this.pageButtons = kb.collectionObservable(this.model.pageButtons);
					this.globalSettingsModel = new (require('globalsettings')).Model();
					this.globalSettingsModel.fetch({
						async: false
					});
					this.maxYearlyClientVisits = ko.observable(this.globalSettingsModel.get('MaxYearlyClientVisits'));
					this.throttledPage.subscribe(this.goToPage);
				}

				ViewModel.prototype.goToPage = function (val) {
					if (val && val > 1 && val < this.pageCount()) {
						return this.changePage({
							number: val
						});
					}
				};

				ViewModel.prototype.load = function () {
					this.page(1);
					return this.model.fetch();
				};

				ViewModel.prototype.changePage = function (vm) {
					if (!vm.number) vm.number = 1;
					this.page(vm.number);
					return this.model.fetch();
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var model;
				model = new Report();
				return new ViewModel(model);
			};

		}).call(this);
	}, "site": function (exports, require, module) {
		(function () {
			var AppManager, Notifications, Site, jqueryExt, koExt,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				_this = this;

			koExt = require('ko.ext');

			jqueryExt = require('jquery.ext');

			Notifications = (function () {

				function Notifications() {
					this.removeNotification = __bind(this.removeNotification, this);
					this.addNotification = __bind(this.addNotification, this); this.items = ko.observableArray([]);
					this.intervals = new Array();
					this.node = $('notifications');
					ko.applyBindings(this.items, this.node[0]);
				}

				Notifications.prototype.addNotification = function (n) {
					var callback, interval,
						_this = this;
					n = $('#' + n).html();
					this.items.push(n);
					callback = function () {
						return _this.removeNotification(n);
					};
					interval = setInterval(callback, 4000);
					return this.intervals.push(interval);
				};

				Notifications.prototype.removeNotification = function (n) {
					var interval;
					this.items.remove(n);
					interval = this.intervals.shift();
					return window.clearInterval(interval);
				};

				return Notifications;

			})();

			AppManager = (function () {

				function AppManager() {
					this.destroyCurrentApp = __bind(this.destroyCurrentApp, this);
					this.startApp = __bind(this.startApp, this);
				}

				AppManager.prototype.startApp = function (appContainerId, appName, params) {
					var $rootScope;
					this.appContainer = document.getElementById(appContainerId);
					if (this.appContainer) {
						this.currentAppName = appName;
						this.currentApp = angular.bootstrap(this.appContainer, [appName]);
						$rootScope = this.currentApp.get('$rootScope');
						_.extend($rootScope, params);
						return $rootScope.$apply();
					}
				};

				AppManager.prototype.destroyCurrentApp = function () {
					var $rootScope;
					if (this.currentApp === null || this.currentApp === void 0) return;
					$rootScope = this.currentApp.get('$rootScope');
					$rootScope.$destroy();
					return $(this.appContainer).empty();
				};

				return AppManager;

			})();

			Site = (function () {

				function Site() {
					this.render = __bind(this.render, this);
					this.onTemplateLoaded = __bind(this.onTemplateLoaded, this);
					this.remoteTemplate = __bind(this.remoteTemplate, this);
					this.loadModule = __bind(this.loadModule, this);
					this.setupNewOBNLReinvestmentWorkflow = __bind(this.setupNewOBNLReinvestmentWorkflow, this);
					this.setupNewInvoiceWorkflow = __bind(this.setupNewInvoiceWorkflow, this);
					this.setupNewInvoiceWorkflow2023 = __bind(this.setupNewInvoiceWorkflow2023, this);
					this.setupNewPocInvoiceWorkflow = __bind(this.setupNewPocInvoiceWorkflow, this);
					this.sendErrorMessage = __bind(this.sendErrorMessage, this);
					this.init = __bind(this.init, this);
				}

				Site.prototype.init = function () {
					var node,
						_this = this;
					koExt.setup();
					jqueryExt.setup();
					this.notifications = new Notifications();
					this.appManager = new AppManager();
					$.ajaxSetup({
						cache: false
					});
					node = $('#app')[0];
					kb.active_transitions = '';
					this.page_navigator = new kb.PageNavigatorPanes(node, {
						no_remove: false
					});
					this.router = new Backbone.Router();
					this.router.route('', null, function () {
						return _this.loadModule('dashboard', 'default_dashboardTemplate', (function (vm) {
							return vm.load();
						}), function () {
							return _this.appManager.startApp('ng-app', 'eco.container');
						});
					});
					this.router.route('giveaway/index', null, function () {
						return _this.loadModule('empty.module', 'giveaway_indexTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.giveaway');
						});
					});
					this.router.route('giveaway/new', null, function () {
						return _this.loadModule('empty.module', 'giveaway_newTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.giveaway');
						});
					});
					this.router.route('giveaway/edit/:id', null, function (id) {
						return _this.loadModule('empty.module', 'giveaway_newTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.giveaway', {
								id: id
							});
						});
					});
					this.router.route('giveaway-type/index', null, function (id) {
						return _this.loadModule('empty.module', 'giveawayType_indexTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.giveawayType');
						});
					});
					this.router.route('container/index', null, function () {
						return _this.loadModule('empty.module', 'container_indexTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.container');
						});
					});
					this.router.route('container/new', null, function () {
						return _this.loadModule('empty.module', 'container_newTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.container');
						});
					});
					this.router.route('container/edit/:id', null, function (id) {
						return _this.loadModule('empty.module', 'container_newTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.container', {
								id: id
							});
						});
					});
					this.router.route('invoice/payment/:id', null, function (id) {
						return _this.loadModule('empty.module', 'payment_indexTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.payment', {
								id: id
							});
						});
					});
					this.router.route('invoice/show/:id', null, function (id) {
						return _this.loadModule('empty.module', 'invoice_showTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.invoice', {
								id: id
							});
						});
					});
					this.router.route('payment/settings', null, function (id) {
						return _this.loadModule('empty.module', 'payment_settingsTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.payment', {
								id: id
							});
						});
					});
					this.router.route('municipality/index', null, function () {
						return _this.loadModule('municipality.list', 'municipality_indexTemplate', function (vm) {
							return vm.load();
						});
					});
					this.router.route('municipality/new', null, function (id) {
						return _this.loadModule('empty.module', 'municipality_newTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.municipality', {
								id: null
							});
						});
					});
					this.router.route('municipality/edit/:id', null, function (id) {
						return _this.loadModule('empty.module', 'municipality_newTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.municipality', {
								id: id
							});
						});
					});
					this.router.route('hub/index', null, function () {
						return _this.loadModule('hub.list', 'hub_indexTemplate', function (vm) {
							return vm.load();
						});
					});
					this.router.route('hub/new', null, function () {
						return _this.loadModule('hub.form', 'hub_newTemplate');
					});
					this.router.route('hub/edit/:id', null, function (id) {
						return _this.loadModule('empty.module', 'hub_newTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.hub', {
								id: id
							});
						});
					});
					this.router.route('client/index', null, function () {
						return _this.loadModule('client.list', 'client_indexTemplate', function (vm) {
							return vm.load();
						});
					});
					this.router.route('client/index/:filter', null, function (filter) {
						return _this.loadModule('client.list', 'client_indexTemplate', function (vm) {
							if (filter === 'currentMonth') {
								vm.lastVisitFrom(Date.today().set({
									day: 1
								}));
							} else if (filter === 'currentYear') {
								vm.lastVisitFrom(Date.today().set({
									day: 1,
									month: 0
								}));
							} else if (filter === 'today') {
								vm.lastVisitFrom(Date.today());
							}
							return vm.load();
						});
					});
					this.router.route('client/inactive', null, function () {
						return _this.loadModule('client.inactive', 'client_indexTemplate', function (vm) {
							return vm.load();
						});
					});
					this.router.route('client/new', null, function () {
						return _this.loadModule('client.form', 'client_newTemplate', function (vm) {
							return vm.loadNew();
						});
					});
					this.router.route('client/newcanadian', null, function () {
						return _this.loadModule('client.canadian.form', 'client_canadian_NewTemplate', function (vm) {
							return vm.loadNew();
						});
					});
					this.router.route('client/edit/:id', null, function (id) {
						return _this.loadModule('client.form', 'client_newTemplate', function (vm) {
							return vm.load(id);
						});
					});
					this.router.route('client/show/:id', null, function (id) {
						return _this.loadModule('client.show', 'client_showTemplate', function (vm) {
							return vm.load(id);
						});
					});
					this.router.route('client/merge', null, function () {
						return _this.loadModule('client.merger', 'client_mergerTemplate');
					});
					this.router.route('invoice/index', null, function () {
						return _this.loadModule('invoice.list', 'invoice_indexTemplate', function (vm) {
							return vm.load();
						});
					});
					this.router.route('invoice/trash', null, function () {
						return _this.loadModule('invoice.list', 'invoice_trashTemplate', function (vm) {
							vm.deleted(true);
							return vm.load();
						});
					});
					this.setupNewInvoiceWorkflow();
					this.setupNewInvoiceWorkflow2023();
					this.setupNewOBNLReinvestmentWorkflow();
					this.setupNewPocInvoiceWorkflow();
					this.router.route('obnlreinvestment/show/:id', null, function (id) {
						return _this.loadModule('obnlreinvestment.show', 'obnlreinvestment_showTemplate', function (vm) {
							return vm.load(id);
						});
					});
					this.router.route('material/index', null, function () {
						return _this.loadModule('material.list', 'material_indexTemplate', function (vm) {
							return vm.load();
						});
					});
					this.router.route('material/new', null, function () {
						return _this.loadModule('empty.module', 'material_newTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.material');
						});
					});
					this.router.route('material/edit/:id', null, function (id) {
						return _this.loadModule('empty.module', 'material_newTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.material', {
								id: id
							});
						});
					});
					this.router.route('material/merge', null, function () {
						return _this.loadModule('material.merger', 'material_mergerTemplate');
					});
					this.router.route('user/index', null, function () {
						return _this.loadModule('user.list', 'user_indexTemplate', function (vm) {
							return vm.load();
						});
					});
					this.router.route('user/new', null, function () {
						return _this.loadModule('user.form', 'user_newTemplate');
					});
					this.router.route('user/edit/:id', null, function (id) {
						return _this.loadModule('user.form', 'user_newTemplate', function (vm) {
							return vm.load(id);
						});
					});
					this.router.route('report/journal', null, function () {
						return _this.loadModule('reports.journal', 'reports_journalTemplate', function (vm) {
							return vm.load();
						});
					});
					this.router.route('report/journal/:filter', null, function (filter) {
						return _this.loadModule('reports.journal', 'reports_journalTemplate', function (vm) {
							if (filter === 'currentMonth') {
								vm.from(Date.today().set({
									day: 1
								}));
							} else if (filter === 'currentYear') {
								vm.from(Date.today().set({
									day: 1,
									month: 0
								}));
							} else if (filter === 'today') {
								vm.from(Date.today());
							}
							return vm.load();
						});
					});
					this.router.route('report/limits', null, function () {
						return _this.loadModule('reports.limits', 'reports_limitsTemplate', function (vm) {
							return vm.load();
						});
					});
					this.router.route('report/visitslimits', null, function () {
						return _this.loadModule('reports.visitslimits', 'reports_visitsLimitsTemplate', function (vm) {
							return vm.load();
						});
					});
					this.router.route('report/materials', null, function (id) {
						return _this.loadModule('empty.module', 'reports_materialsTemplate', null, function () {
							return _this.appManager.startApp('ng-app', 'eco.material-report');
						});
					});
					this.router.route('report/materialsbyaddress', null, function () {
						return _this.loadModule('reports.materialsbyaddress', 'reports_materialsByAddressTemplate', function (vm) {
							return {};
						});
					});
					this.router.route('report/obnltotal', null, function () {
						return _this.loadModule('reports.obnltotal', 'reports_OBNLTotalTemplate', function (vm) {
							return vm.load();
						});
					});
					this.router.route('report/obnlglobal', null, function () {
						return _this.loadModule('reports.obnlglobal', 'reports_OBNLGlobalTemplate', function (vm) {
							return vm.load();
						});
					});
					this.router.route('globalsettings/index', null, function () {
						return _this.loadModule('globalsettings.form', 'globalsettings_mainForm', function (vm) {
							return vm.load();
						});
					});
					this.router.route('globalsettings/printerTest', null, function () {
						return _this.loadModule('empty.module', 'globalsettings_printerTest', function (vm) {
							return {};
						});
					});
					$(document).ajaxError(function (event, jqXHR, ajaxSettings, thrownError) {
						if (jqXHR.status !== 401) return;
						return window.location = '/user/login';
					});
					window.onerror = this.sendErrorMessage;
					return Backbone.history.start({
						hashChange: true
					});
				};

				Site.prototype.sendErrorMessage = function (errorMsg, url, lineNumber) {
					return $.post('default/logError', {
						error: errorMsg,
						url: url,
						lineNumber: lineNumber
					});
				};

				Site.prototype.setupNewInvoiceWorkflow = function () {
					var navigate, workflow,
						_this = this;
					navigate = function (route) {
						_this.router.navigate(route, {
							trigger: true,
							replace: true
						});
						return false;
					};
					workflow = new (require('invoice.workflow')).Workflow(navigate, this.remoteTemplate);
					this.router.route('invoice/new', null, workflow.newInvoice);
					this.router.route('invoice/new/form', null, workflow.showForm);
					this.router.route('invoice/newPocPage/form', null, workflow.showPocForm);
					this.router.route('invoice/new/client/change', null, workflow.clientChange);
					this.router.route('invoice/new/client/create', null, workflow.clientCreate);
					return this.router.route('invoice/new/client/edit/:id', null, workflow.clientEdit);
				};

				Site.prototype.setupNewInvoiceWorkflow2023 = function () {
					var navigate, workflow,
						_this = this;
					navigate = function (route) {
						_this.router.navigate(route, {
							trigger: true,
							replace: true
						});
						return false;
					};
					workflow = new (require('invoice.workflow')).Workflow(navigate, this.remoteTemplate);
					this.router.route('invoice/new2023', null, workflow.newInvoice2023);
					this.router.route('invoice/new/form2023', null, workflow.showForm2023);
					this.router.route('invoice/new/client/change', null, workflow.clientChange);
					this.router.route('invoice/new/client/create', null, workflow.clientCreate);
					return this.router.route('invoice/new/client/edit/:id', null, workflow.clientEdit);
				};

				Site.prototype.setupNewPocInvoiceWorkflow = function () {
					var navigate, workflow,
						_this = this;
					navigate = function (route) {
						_this.router.navigate(route, {
							trigger: true,
							replace: true
						});
						return false;
					};
					workflow = new (require('invoice.workflow')).Workflow(navigate, this.remoteTemplate);
					this.router.route('invoice/newPocPage', null, workflow.newPocInvoice);
					this.router.route('invoice/newPocPage/form', null, workflow.showPocForm);
					this.router.route('invoice/new/client/change', null, workflow.clientChange);
					this.router.route('invoice/new/client/create', null, workflow.clientCreate);
					return this.router.route('invoice/new/client/edit/:id', null, workflow.clientEdit);
				};

				Site.prototype.setupNewOBNLReinvestmentWorkflow = function () {
					var navigate, workflow,
						_this = this;
					navigate = function (route) {
						_this.router.navigate(route, {
							trigger: true,
							replace: true
						});
						return false;
					};
					workflow = new (require('obnlreinvestment.workflow')).Workflow(navigate, this.remoteTemplate);
					this.router.route('obnlreinvestment/new', null, workflow.newOBNLReinvestment);
					this.router.route('obnlreinvestment/new/form', null, workflow.showForm);
					this.router.route('obnlreinvestment/new/client/change', null, workflow.clientChange);
					this.router.route('obnlreinvestment/new/client/create', null, workflow.clientCreate);
					return this.router.route('obnlreinvestment/new/client/edit/:id', null, workflow.clientEdit);
				};

				Site.prototype.loadModule = function (module, template, callback, templateLoadedCallback) {
					var vm;
					if (callback == null) callback = null;
					if (templateLoadedCallback == null) templateLoadedCallback = null;
					this.appManager.destroyCurrentApp();
					module = require(module);
					vm = module.createViewModel();
                    this.remoteTemplate(template, vm, templateLoadedCallback);
                    if (vm ?.model ?.url && typeof (vm.model.url) !== "function" && vm.model.url.includes("materialsbyaddress")) {
                        vm.search();
                    }
					if (callback !== null) return callback(vm);
				};

				Site.prototype.remoteTemplate = function (name, vm, callback) {
					var url,
						_this = this;
					$('#app').hide();
					$('#loading').show();
					if ($('#' + name).length < 1) {
						url = name.replace('_', '/');
						return $.get(url, {}, function (t) {
							_this.onTemplateLoaded(t, name);
							_this.render(name, vm);
							if (callback) return callback();
						});
					} else {
						this.render(name, vm);
						if (callback) return callback();
					}
				};

				Site.prototype.onTemplateLoaded = function (template, name) {
					template = '<script type="text/html" id="' + name + '">' + template + '</script>';
					return $('body').append(template);
				};

				Site.prototype.render = function (name, viewModel) {
					var input;
					$('#app').show();
					$('#loading').hide();
					this.page_navigator.loadPage(kb.renderTemplate(name, viewModel));
					input = $('input');
					$('.postalcode-mask').mask('S0S-0S0');
					$('.visits-limit-mask').mask('000');
					$('.civic-number-autocomplete').on('focus', function () {
						$(this).autocomplete("search", '');
					});
					$('.street-name-autocomplete').on('focus', function () {
						$(this).autocomplete("search", '');
					});
					$('.postal-code-autocomplete').on('focus', function () {
						$(this).autocomplete("search", '');
					});
					return input.simplePlaceholder();
				};

				return Site;

			})();

			$(document).ready(function () {
				var site;
				site = new Site();
				site.init();
				return eco.app = site;
			});

		}).call(this);
	}, "user": function (exports, require, module) {
		(function () {
			var CurrentUserModel, NewUserModel, UserList, UserModel,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

			UserModel = (function (_super) {

				__extends(UserModel, _super);

				UserModel.prototype.idAttribute = 'Id';

				function UserModel(data) {
					this.url = __bind(this.url, this); UserModel.__super__.constructor.call(this, data);
				}

				UserModel.prototype.url = function () {
					if (this.isNew()) return "/user";
					return "/user/index/" + this.id;
				};

				return UserModel;

			})(Backbone.Model);

			CurrentUserModel = (function (_super) {

				__extends(CurrentUserModel, _super);

				CurrentUserModel.prototype.idAttribute = 'Id';

				function CurrentUserModel(data) {
					this.url = __bind(this.url, this); CurrentUserModel.__super__.constructor.call(this, data);
				}

				CurrentUserModel.prototype.url = function () {
					return "/user/current";
				};

				return CurrentUserModel;

			})(Backbone.Model);

			NewUserModel = (function (_super) {

				__extends(NewUserModel, _super);

				function NewUserModel() {
					NewUserModel.__super__.constructor.call(this, {
						Id: null,
						HubId: '',
						Login: '',
						Email: '',
						Password: '',
						PasswordConfirmation: '',
						IsReadOnly: false,
						IsAdmin: false,
						IsGlobalAdmin: false,
						IsAgent: true,
						Name: "",
						IsLoginAlertEnabled: false
					});
				}

				return NewUserModel;

			})(UserModel);

			UserList = (function (_super) {

				__extends(UserList, _super);

				function UserList(models, options) {
					this.url = '/user';
					this.model = UserModel;
					UserList.__super__.constructor.call(this, models, options);
				}

				return UserList;

			})(Backbone.Collection);

			exports.ListModel = UserList;

			exports.Model = UserModel;

			exports.NewModel = NewUserModel;

			exports.CurrentUserModel = CurrentUserModel;

		}).call(this);
	}, "user.form": function (exports, require, module) {
		(function () {
			var ViewModel,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				__hasProp = Object.prototype.hasOwnProperty,
				__extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
				_this = this;

			ViewModel = (function (_super) {

				__extends(ViewModel, _super);

				function ViewModel(model, hubsModel) {
					this.onError = __bind(this.onError, this);
					this.onSaved = __bind(this.onSaved, this);
					this.save = __bind(this.save, this);
					this.load = __bind(this.load, this);
					this.updateNew = __bind(this.updateNew, this);
					var isAdminTrueObject;
					ViewModel.__super__.constructor.call(this, model);
					this.errors = ko.observableArray([]);
					this.isNew = ko.observable(true);
					this.hubs = kb.collectionObservable(hubsModel.hubs);
					isAdminTrueObject = {
						read: function () {
							return (this.IsAdmin() === true).toString();
						},
						write: function (val) {
							if (val === "true") {
								return this.IsAdmin(true);
							} else {
								return this.IsAdmin(false);
							}
						}
					};
					this.isAdminTrue = ko.computed(isAdminTrueObject, this);
					model.on('change', this.updateNew);
				}

				ViewModel.prototype.updateNew = function () {
					this.IsAdmin(!!this.IsAdmin());
					this.IsAgent(!(!!this.IsAdmin()));
					if (this.IsAgent()) this.IsGlobalAdmin(false);
					return this.isNew(this.model().isNew());
				};

				ViewModel.prototype.load = function (id) {
					return this.model().fetch({
						data: {
							id: id
						}
					});
				};

				ViewModel.prototype.save = function () {
					return this.model().save(null, {
						success: this.onSaved,
						error: this.onError
					});
				};

				ViewModel.prototype.onSaved = function (data) {
					return eco.app.router.navigate('user/index', {
						trigger: true
					});
				};

				ViewModel.prototype.onError = function (m, errors, data) {
					var item, _i, _len, _results;
					errors = $.parseJSON(errors.responseText);
					this.errors.removeAll();
					_results = [];
					for (_i = 0, _len = errors.length; _i < _len; _i++) {
						item = errors[_i];
						_results.push(this.errors.push(item));
					}
					return _results;
				};

				return ViewModel;

			})(kb.ViewModel);

			exports.createViewModel = function () {
				var hubsModel, model;
				model = new (require('user')).NewModel();
				hubsModel = new (require('hub')).ListModel();
				hubsModel.fetch();
				return new ViewModel(model, hubsModel);
			};

		}).call(this);
	}, "user.list": function (exports, require, module) {
		(function () {
			var UserViewModel, ViewModel, common,
				__bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; },
				_this = this;

			common = require('common');

			UserViewModel = (function () {

				function UserViewModel(data) {
					this.model = data;
					this.Login = data.get('Login');
					this.Id = data.get('Id');
					this.IsAdmin = kb.observable(data, {
						key: 'IsAdmin',
						localizer: common.LocalizedStringLocalizer
					});
					this.IsGlobalAdmin = kb.observable(data, {
						key: 'IsGlobalAdmin',
						localizer: common.LocalizedStringLocalizer
					});
					this.IsReadOnly = kb.observable(data, {
						key: 'IsReadOnly',
						localizer: common.LocalizedStringLocalizer
					});
					this.IsLoginAlertEnabled = kb.observable(data, {
						key: 'IsLoginAlertEnabled',
						localizer: common.LocalizedStringLocalizer
					});
				}

				return UserViewModel;

			})();

			ViewModel = (function () {

				function ViewModel(model) {
					this.onRemove = __bind(this.onRemove, this);
					this.onEdit = __bind(this.onEdit, this);
					this.onNew = __bind(this.onNew, this);
					this.search = __bind(this.search, this);
					this.changePage = __bind(this.changePage, this);
					this.load = __bind(this.load, this); this.model = model;
					this.items = kb.collectionObservable(this.model, {
						view_model: UserViewModel
					});
				}

				ViewModel.prototype.load = function () {
					return this.model.fetch();
				};

				ViewModel.prototype.changePage = function (vm) {
					return this.model.changePage(vm.number);
				};

				ViewModel.prototype.search = function () {
					return this.model.search();
				};

				ViewModel.prototype.onNew = function () {
					return eco.app.router.navigate('user/new', {
						trigger: true
					});
				};

				ViewModel.prototype.onEdit = function (vm) {
					return eco.app.router.navigate('user/edit/' + vm.Id, {
						trigger: true
					});
				};

				ViewModel.prototype.onRemove = function (vm) {
					if (!confirm(kb.locale_manager.get("Do you want to remove the user?"))) {
						return;
					}
					return vm.model.destroy();
				};

				return ViewModel;

			})();

			exports.createViewModel = function () {
				var model;
				model = new (require('user')).ListModel();
				return new ViewModel(model);
			};

		}).call(this);
	}
});