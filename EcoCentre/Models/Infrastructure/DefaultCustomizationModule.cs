using System.Web.Mvc;
using Autofac;
using EcoCentre.Models.ViewModel;

namespace EcoCentre.Models.Infrastructure
{
    public class DefaultCustomizationModule : Module
    {
        public string ViewRootDir { get; set; }
        public string PageTitle { get; set; }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomizationProvider>().AsImplementedInterfaces().
                OnActivating(x => x.Instance.PageTitle = PageTitle);
            builder.Register(x => new EcoRazorViewEngine(ViewRootDir)).As<RazorViewEngine>().SingleInstance();
        }
    }
}