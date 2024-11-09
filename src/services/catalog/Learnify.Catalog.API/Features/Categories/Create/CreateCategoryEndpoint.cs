﻿namespace Learnify.Catalog.API.Features.Categories.Create;

public static class CreateCategoryEndpoint
{
    public static RouteGroupBuilder CreateCategoryGroupItemEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/", async (CreateCategoryCommand command, IMediator mediator) 
            => await mediator.Send(command).ToGenericResultAsync());

        routeGroupBuilder.WithName("CreateCategory");
        routeGroupBuilder.AddEndpointFilter<ValidationFilter<CreateCategoryCommand>>();

        return routeGroupBuilder;
    } 
}
