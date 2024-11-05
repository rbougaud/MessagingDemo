namespace Application.Common.Dto.MoviesCommand;

public readonly record struct MovieCommandDto(Guid OrderId, Guid MovieId, int Quantity);
