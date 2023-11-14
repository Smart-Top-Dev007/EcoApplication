(function () {
	'use strict';

	angular
		.module('eco.invoice')
		.component('invoicePage', {
			templateUrl: '/template/invoice/invoice-page',
			controllerAs: 'vm',
			controller: invoicePage,
			bindings: {
				invoiceId: '='
			}
		});

	invoicePage.$inject = ['invoiceService', 'paymentService', 'errorHandler'];
	function invoicePage(invoiceService, paymentService, errorHandler) {

		var vm = this;
		vm.isLoading = false;
		vm.isPaymentLoading = false;
		vm.isDeleting = false;
		vm.invoice = null;
		vm.isDeleted = false;
		vm.payWithCash = payWithCash;
		vm.deleteInvoice = deleteInvoice;
		vm.sendToQuickPrinter = sendToQuickPrinter;
		vm.goBack = goBack;
		vm.canBePaid = canBePaid;
		vm.payWithCredit = payWithCredit;
		vm.canBePaidInCredit = canBePaidInCredit;
		vm.canBePrinted = canBePrinted;

		var paymentTypes = null;

		vm.$onInit = init;

		function init() {
			vm.isLoading = true;

			loadInvoice()
				.then(loadPaymentTypes)
				.catch(handleInvoiceLoadingErrors)
				.finally(function() {
					vm.isLoading = false;
				});
		};

		function loadInvoice() {
			return invoiceService.getInvoice(vm.invoiceId)
				.then(function(response) {
					vm.invoice = response.data;
				});
		}

		function loadPaymentTypes() {
			return invoiceService.getPaymentTypes()
				.then(function(response) {
					paymentTypes = toDict(response.data , 'id');
				});
		}

		function canBePaid() {
			return !vm.invoice.payment && vm.invoice.amountIncludingTaxes;
		}

		function canBePrinted() {
			return vm.invoice.payment || !vm.invoice.amountIncludingTaxes;
		}

		function canBePaidInCredit() {
			return canBePaid() && vm.invoice.client.allowCredit;
		}

		function goBack() {
			window.history.back();
		}

		function toDict(source, keySelector) {
			return _.reduce(source || [],
				function (memo, s) {
					var key = s[keySelector];
					memo[key] = s;
					return memo;
				},
				{});
		}


		function payWithCash() {

			if (!confirm("Êtes-vous sûr?")) {
				return;
			}
			
			var request = {
				invoiceId: vm.invoiceId
			};

			vm.isPaymentLoading = true;
			paymentService.payWithCash(request)
				.then(function (response) {
					vm.invoice.payment = response.data;
				})
				.catch(errorHandler)
				.finally(function () {
					vm.isPaymentLoading = false;
				});
		}

		function payWithCredit() {
			var request = {
				invoiceId: vm.invoiceId
			};
			vm.isPaymentLoading = true;
			paymentService.payWithCredit(request)
				.then(function (response) {
					vm.invoice.payment = response.data;
				})
				.catch(errorHandler)
				.finally(function () {
					vm.isPaymentLoading = false;
				});
		}

		function handleInvoiceLoadingErrors(response) {
			if (response.status == 404) {
				errorHandler("Invoice not found");
			} else {
				errorHandler(response);
			}
		}

		function deleteInvoice() {

			if (!confirm("Voulez-vous supprimer cette facture?")) {
				return;
			}

			vm.isDeleting = true;
			invoiceService.deleteInvoice(vm.invoiceId)
				.then(function (response) {
					vm.isDeleted = true;
				})
				.catch(errorHandler)
				.finally(function () {
					vm.isDeleting = false;
				});
		}

		function sendToQuickPrinter() {

			var invoiceText = getQuickPrinterText(vm.invoice);

			// enable fast printing; without it printing is 'saccade'.
			// print invoice twice: one copy for client, one for attendant.
			var text = "<printer fast_mode='true'>" + 
				invoiceText +
				invoiceText;

			var textEncoded = encodeURI(text);
			
			window.location.href = "quickprinter://" + textEncoded;

			console.log(text);
		}
		
		function floatRight(line, textToFloat, buffer) {
			// given the line, add the "textToFloat"
			// so that it would be printed as if it's
			// on the right.
			// Buffer is minimal number of spaces between 2 params.
			// e.g.
			// "Wood chippings", "$1.00" should result in
			// "Wood chippings                 $1.00"

			// "Wood chippings with a very descriptive name", "$1.00" should result in
			// "Wood chippings with a very     $1.00" + "<br>" +
			// "descriptive name"

			buffer = buffer || 3;

			line = line + "";
			textToFloat = textToFloat + "";

			// there are 42 chars in one line on epson printer...
			var lineWidth = 42;

			var maxCharsInLine = lineWidth - buffer - textToFloat.length;
			var words = line.split(" ");
			var currentLineIndex = 0;
			var result = "";
			var charsInCurrentLine = 0;

			var i = 0;
			while (i < words.length) {
				if (words[i].length >= maxCharsInLine) {
					charsInCurrentLine = charsInCurrentLine % maxCharsInLine;
					result += word[i];
					i++;
					continue;
				}
				
				if (charsInCurrentLine + words[i].length < maxCharsInLine) {
					charsInCurrentLine += words[i].length + 1;
					result += words[i] + " ";
					i++;
				} else {
					if (currentLineIndex == 0) {
						var spaceCount = lineWidth - charsInCurrentLine - textToFloat.length;
						result += " ".repeat(spaceCount);
						result += textToFloat;
					}
					if (i + 1 < words.length) {
						result += "<br>";
					}
					charsInCurrentLine = 0;
					currentLineIndex++;

				}

			}

			if (currentLineIndex == 0) {
				var spaceCount2 = lineWidth - charsInCurrentLine - textToFloat.length;
				result += " ".repeat(spaceCount2);
				result += textToFloat;
			}
			
			return result;
		}

		function getQuickPrinterText() {
			
			var result = "";
			
			result += "<center>COMPO RECYCLE<br><br>";
			result += "<normal>";
			if (vm.invoice.center && vm.invoice.center.address) {
				result += vm.invoice.center.address.split('\n').join("<br>");
			}
			result += "<br>";
			result += "Facture: " + vm.invoice.invoiceNo + "<br>";
			result += "Date: " + moment(vm.invoice.createdAt).format("YYYY-MM-DD HH:mm") + "<br>";
			result += "<br>";


			var client = vm.invoice.client;
			result += client.firstName + " " + client.lastName;
			result += "<br>";

			var address = vm.invoice.address;

			result += address.civicNumber;
			if (address.aptNumber) {
				result += " - "+ address.aptNumber;
			}
			result += ", " + address.street;
			result += "<br>";

			result += address.postalCode + ", " + address.city;
			result += "<br>";

			/*
			 if (client.phoneNumber) {
				result += "P: " + client.phoneNumber;
				result += "<br>";
			}
			*/

			result += "<br>";
			if (vm.invoice.materials) {
				result += _.map(vm.invoice.materials,
					function (material) {
						return floatRight(material.name, "$" + material.amount.toFixed(2)) + "<br>  " + material.quantity + " " + material.unit + "<br>";
					}).join("");
			}
			if (vm.invoice.giveawayItems) {
				result += _.map(vm.invoice.giveawayItems,
					function(item) {
						return floatRight(item.title, "$" + item.price.toFixed(2)) + "<br>";
					}).join("");
			}

			result += "<DLINE><br>";
			result += floatRight("Sous-total", "$" + vm.invoice.amount.toFixed(2)) +"<br>";
			if (vm.invoice.taxes) {
				result += _.map(vm.invoice.taxes,
					function (item) {
						return floatRight(item.name, "$" + item.amount.toFixed(2));
					}).join("<br>");
			}
			result += "<br><LINE><br>";
			result += floatRight("Total", "$" + vm.invoice.amountIncludingTaxes.toFixed(2));
			result += "<br>";
			result += "<br>";
			result += "Visite #" + vm.invoice.visitNumber;
			if (vm.invoice.visitLimit) {
				result += " / " + vm.invoice.visitLimit;
			}
			result += "<br>";
			if (vm.invoice.createdBy) {
				result += "Créé par: " + (vm.invoice.createdBy.userName || 'deleted user')+"<br>";
			}
			result += "<br>";

			var payment = vm.invoice.payment;
			if (payment) {

				result += "<br><LINE><br>";
				var paymentType = paymentTypes[payment.paymentMethod] || {};
				result += "Type de paiement: " + paymentType.description + "<br>";
				result += "Date: " + moment(payment.dateProcessed).format("YYYY-MM-DD HH:mm") + "<br>";
				if (payment.reference) {
					result += "Référence: " + payment.reference + "<br>";
				}
				result += "Traité par: " + payment.processedByUser.name + "<br>";
				result += "<br>";
			}
			result += "<br>";
			result += "<cut>";
			return result;
		}
	}
})();
