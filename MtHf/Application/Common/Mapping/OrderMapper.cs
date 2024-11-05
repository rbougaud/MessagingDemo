using Application.Common.Dto.Movies;
using Application.Common.Dto.MoviesCommand;
using Application.Common.Dto.Orders;
using Contracts.Order;
using Domain.Abstraction.Customers;
using Domain.Common.Enums;
using Domain.Entities;
using Domain.Entities.Projections;

namespace Application.Common.Mapping;

public static class OrderMapper
{
    public static Order ToDao(this OrderCreated orderCreated, Customer customer)
    {
        ICollection<MovieCommand> collection = [];
        AddMoviesToOrder(orderCreated, collection);
        return new Order
        {
            Id = orderCreated.Id,
            DateOrder = orderCreated.DateOrder,
            DueDate = orderCreated.DueDate,
            PaymentMode = (short)PaymentMode.None,
            DeliveryMode = orderCreated.DeliveryMode,
            State = orderCreated.State,
            CustomerId = orderCreated.CustomerId,
            Customer = customer,
            MovieCommands = collection
        };
    }

    public static Order ToDao(this OrderCreated orderCreated)
    {
        ICollection<MovieCommand> collection = [];
        AddMoviesToOrder(orderCreated, collection);

        return new Order
        {
            Id = orderCreated.Id,
            DateOrder = orderCreated.DateOrder,
            DueDate = orderCreated.DueDate,
            PaymentMode = (short)PaymentMode.None,
            DeliveryMode = orderCreated.DeliveryMode,
            State = orderCreated.State,
            CustomerId = orderCreated.CustomerId,
            MovieCommands = collection
        };
    }

    private static void AddMoviesToOrder(OrderCreated orderCreated, ICollection<MovieCommand> collection)
    {
        if (orderCreated.OrderMovies.Count != 0)
        {
            foreach (var item in orderCreated.OrderMovies)
            {
                var movieCmd = new MovieCommand
                {
                    MovieId = item.Key,
                    Quantity = item.Value,
                    OrderId = orderCreated.Id
                };
                collection.Add(movieCmd);
            }
        }
    }

    public static OrderDto ToDto(this OrderCreated orderCreated, ICustomerDto customerDto)
    {
        return new OrderDto
        {
            Id = orderCreated.Id,
            DateOrder = orderCreated.DateOrder,
            DueDate = orderCreated.DueDate,
            PaymentMode = (short)PaymentMode.None,
            DeliveryMode = orderCreated.DeliveryMode,
            State = orderCreated.State,
            CustomerDto = customerDto
        };
    }

    public static OrderDto ToDto(this OrderProjection orderProjection)
    {
        return new OrderDto
        {
            Id = orderProjection.Id,
            DateOrder = orderProjection.DateOrder,
            DueDate = orderProjection.DueDate,
            DeliveryMode = orderProjection.DeliveryMode,
            State = orderProjection.State,
            PaymentMode = orderProjection.PaymentMode,
            DatePayment = orderProjection.DatePayment,
            DeliveryDate = orderProjection.DeliveryDate,
            CustomerDto = orderProjection.Customer.ToDto(),
            MovieCommandDtos = orderProjection.MovieCommands.ToDto()
        };
    }

    private static MovieCommandFullInfoDto[] ToDto(this ICollection<MovieCommandProjection> movieCommandProjections)
    {
        return movieCommandProjections.Select(x => new MovieCommandFullInfoDto((MovieDto)x.Movie.ToDto(), x.Quantity)).ToArray();
    }
}
