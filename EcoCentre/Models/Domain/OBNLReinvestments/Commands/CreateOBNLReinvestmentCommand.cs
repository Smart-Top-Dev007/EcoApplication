using System;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.OBNLReinvestments.Events;
using EcoCentre.Models.Domain.User;
using FluentValidation;
using MassTransit;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Commands
{
    public class CreateOBNLReinvestmentCommand
    {
        private readonly AuthenticationContext _authenticationContext;
        private readonly CenterIdentification _centerIdentification;
        private readonly Repository<Client> _clientRepository;
        private readonly FileRepository _fileRepository;
        private readonly Repository<OBNLReinvestment> _obnlReinvestmentRepository;
        private readonly OBNLReinvestmentViewModelValidator _obnlReinvestmentViewModelValidator;
        private readonly Sequences _sequences;
        private readonly IServiceBus _serviceBus;

        public CreateOBNLReinvestmentCommand(Repository<OBNLReinvestment> obnlReinvestmentRepository,
            OBNLReinvestmentViewModelValidator obnlReinvestmentViewModelValidator,
            Sequences sequences,
            FileRepository fileRepository,
            Repository<Client> clientRepository,
            IServiceBus serviceBus,
            AuthenticationContext authenticationContext,
            CenterIdentification centerIdentification)
        {
            _obnlReinvestmentRepository = obnlReinvestmentRepository;
            _obnlReinvestmentViewModelValidator = obnlReinvestmentViewModelValidator;
            _sequences = sequences;
            _fileRepository = fileRepository;
            _clientRepository = clientRepository;
            _serviceBus = serviceBus;
            _authenticationContext = authenticationContext;
            _centerIdentification = centerIdentification;
        }

        public OBNLReinvestment Execute(OBNLReinvestmentViewModel vm)
        {
            _obnlReinvestmentViewModelValidator.ValidateAndThrow(vm);
            var client = _clientRepository.FindOne(vm.ClientId);

            var obnlReinvestment = new OBNLReinvestment
            {
                CreatedAt = DateTime.UtcNow,
                CreatedBy = new OBNLReinvestmentCreator
                {
                    UserId = _authenticationContext.User.Id,
                    UserName = _authenticationContext.User.Login
                },
                Comment = vm.Comment,
                ClientId = vm.ClientId,
                Address = client.Address,
                SequentialNo = _sequences.NextOBNLReinvestment(),
                Center = _centerIdentification
            };

            foreach (var material in vm.Materials)
            {
                obnlReinvestment.Materials.Add(new OBNLReinvestmentMaterial
                {
                    MaterialId = material.Id,
                    Weight = material.Weight
                });
            }
            if (vm.Attachments != null)
                foreach (var attachment in vm.Attachments)
                {
                    var file = _fileRepository.Find(attachment.Id);
                    obnlReinvestment.AddAttachment(file.Id.ToString(), file.Filename);
                }

            _obnlReinvestmentRepository.Save(obnlReinvestment);

            _serviceBus.Publish(new OBNLReinvestmentAddedEvent
            {
                InvoiceId = obnlReinvestment.Id
            });
            return obnlReinvestment;
        }
    }
}