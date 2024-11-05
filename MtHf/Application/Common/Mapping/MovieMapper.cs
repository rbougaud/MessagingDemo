using Application.Common.Dto.Movies;
using Contracts.Movie;
using Domain.Abstraction.Movies;
using Domain.Entities;
using Domain.Entities.Projections;

namespace Application.Common.Mapping;

public static class MovieMapper
{
    public static Movie ToDao(this MovieCreated movieCreated)
    {
        return new Movie
        {
            Id = movieCreated.Id,
            Author = movieCreated.Author,
            Title = movieCreated.Title,
            ReleaseDate = movieCreated.ReleaseDate,
            PurchasePrice = movieCreated.PurchasePrice,
            SalePrice = movieCreated.SalePrice
        };
    }

    public static Movie ToDao(this MovieUpdated movieUpdated)
    {
        return new Movie
        {
            Id = movieUpdated.Id,
            Author = movieUpdated.Author,
            Title = movieUpdated.Title,
            ReleaseDate = movieUpdated.ReleaseDate,
            PurchasePrice = movieUpdated.PurchasePrice,
            SalePrice = movieUpdated.Saleprice
        };
    }

    public static IMovieDto ToDto(this MovieProjection movieProjection)
    {
        if (movieProjection is null) { return new MovieDtoNotFound(); }
        return new MovieDto
        {
            Id = movieProjection.Id,
            Author = movieProjection.Author,
            Title = movieProjection.Title,
            ReleaseDate = movieProjection.ReleaseDate,
            PurchasePrice = movieProjection.PurchasePrice,
            SalePrice = movieProjection.SalePrice
        };
    }
}
