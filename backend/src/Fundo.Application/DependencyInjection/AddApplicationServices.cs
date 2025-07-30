using FluentValidation;
using Fundo.Application.Behaviors;
using Fundo.Application.Commands.Loans.Create;
using Fundo.Application.Commands.Loans.Payment;
using Fundo.Application.Commands.Loans.RegisterPayment;
using Fundo.Application.Queries.Loan.GetById;
using Fundo.Application.Queries.Loan.List;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fundo.Application.DependencyInjection;

public static class AddApplicationServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<CreateLoanCommandHandler>();
            cfg.RegisterServicesFromAssemblyContaining<AddPaymentCommandHandler>();
            cfg.RegisterServicesFromAssemblyContaining<RegisterPaymentCommandHandler>();
            cfg.RegisterServicesFromAssemblyContaining<GetAllLoansQueryHandler>();
            cfg.RegisterServicesFromAssemblyContaining<GetLoanByIdQueryHandler>();
        });

        services.AddValidatorsFromAssemblyContaining<CreateLoanCommandHandler>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}