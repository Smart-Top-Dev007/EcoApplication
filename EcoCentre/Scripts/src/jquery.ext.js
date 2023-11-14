function setup() {
	$.fn.loading = function () {
		this.data("loading-text", "<i class='fa fa-spinner fa-spin'></i>  " + this.html()).button("loading");
	};

	$.fn.stopLoading = function () {
		this.button("reset");
	};
}
exports.setup = setup