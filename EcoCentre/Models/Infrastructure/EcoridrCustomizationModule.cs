using Autofac;
using EcoCentre.Models.ViewModel.MainMenu;

namespace EcoCentre.Models.Infrastructure
{
	public class EcoridrCustomizationModule : DefaultCustomizationModule
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterType<MenuProvider>().AsSelf();
			builder.RegisterType<EcoridrMenuProvider>().As<IMenuProvider>();
		}
	}
}