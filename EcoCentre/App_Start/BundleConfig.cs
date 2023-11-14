using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Optimization;

namespace EcoCentre
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif

			var scriptBundle = new ScriptBundle("~/scripts/ng-apps.js") {Orderer = new ModuleBundleOrderer()};
			scriptBundle.IncludeDirectory("~/scripts/ng/", "*.js", true);

			bundles.Add(scriptBundle);


			var libsBundle = new ScriptBundle("~/scripts/libs.js") {Orderer = new NonOrderingBundleOrderer()};
			libsBundle.Include("~/Scripts/lib/jquery-1.9.1.js");
			libsBundle.Include("~/Scripts/lib/Chart.min.js");
			libsBundle.Include("~/Scripts/lib/jquery-ui-1.9.2.custom.min.js");
			libsBundle.Include("~/Scripts/lib/date.js");
			libsBundle.Include("~/Scripts/lib/bootstrap.js");
			libsBundle.Include("~/Scripts/lib/underscore.min.js");
			libsBundle.Include("~/Scripts/lib/backbone.min.js");
			libsBundle.Include("~/Scripts/lib/deep-model.js");
			libsBundle.Include("~/Scripts/lib/knockout-3.2.0.js");
			libsBundle.Include("~/Scripts/lib/knockback.min.js");
			libsBundle.Include("~/Scripts/lib/jquery.mask.min.js");
			libsBundle.Include("~/Scripts/lib/underscore.mixin.deepExtend.js");
			libsBundle.Include("~/Scripts/lib/jquery.upload/js/vendor/jquery.ui.widget.js");
			libsBundle.Include("~/Scripts/lib/jquery.upload/js/jquery.fileupload.js");
			libsBundle.Include("~/Scripts/lib/jquery.upload/js/jquery.iframe-transport.js");
			libsBundle.Include("~/Scripts/lib/jquery.upload/js/jquery.fileupload-ui.js");
			libsBundle.Include("~/Scripts/lib/jquery.upload/js/jquery.fileupload-fp.js");
			libsBundle.Include("~/Scripts/lib/jquery.watermark.min.js");
			libsBundle.Include("~/Scripts/lib/chosen.jquery.min.js");
			libsBundle.Include("~/Scripts/lib/angular.js");
			libsBundle.Include("~/Scripts/lib/bignumber.min.js");
			libsBundle.Include("~/Scripts/lib/knockback-page-navigator-panes.js");
			libsBundle.Include("~/Scripts/lib/moment.js");
			libsBundle.Include("~/Scripts/lib/moment.fr-ca.js");
			libsBundle.Include("~/Scripts/lib/angular-moment.min.js");
			libsBundle.Include("~/Scripts/lib/angular-upload.min.js");
			libsBundle.Include("~/Scripts/lib/angular-file-saver.bundle.js");
			libsBundle.Include("~/Scripts/lib/angular-sanitize.js");
			libsBundle.Include("~/Scripts/lib/angular-chosen.min.js");
			libsBundle.Include("~/Scripts/lib/ngToast.min.js");
			libsBundle.Include("~/Scripts/lib/ui-bootstrap-custom-tpls-2.5.0.js");
			libsBundle.Include("~/Scripts/lib/ng-tags-input.min.js");
			
			bundles.Add(libsBundle);

			var styleBundle = new StyleBundle("~/styles/eco-bundle.css");
			styleBundle.Include("~/styles/eco.css");
			bundles.Add(styleBundle);
		}
		
		class ModuleBundleOrderer : IBundleOrderer
		{
			
			public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
			{
				return files.OrderBy(x => x.VirtualFile.Name != "module.js");
			}
		}

		class NonOrderingBundleOrderer : IBundleOrderer
		{
			public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
			{
				return files;
			}
		}
	}
}
