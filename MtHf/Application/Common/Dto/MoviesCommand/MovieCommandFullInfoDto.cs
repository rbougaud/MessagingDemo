using Application.Common.Dto.Movies;

namespace Application.Common.Dto.MoviesCommand;

public record MovieCommandFullInfoDto(MovieDto MovieDto, int Quantity);
