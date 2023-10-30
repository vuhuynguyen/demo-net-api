using ApplicationCore.Domain.Entities.Room;
using ApplicationCore.Domain.Repositories.Rooms;
using ApplicationCore.Domain.Shared.Exceptions;
using FluentValidation;
using MediatR;

namespace ApplicationCore.Command.Features.Rooms.Commands;

public class AddRoomTypeCommand : IRequest<int>
{
    public int Capacity { get; set; }
    public string Name { get; set; }

    private class Handler : IRequestHandler<AddRoomTypeCommand, int>
    {
        private readonly IRoomTypeRepository _roomTypeRepo;

        public Handler(IRoomTypeRepository roomTypeRepo)
        {
            _roomTypeRepo = roomTypeRepo;
        }

        public async Task<int> Handle(AddRoomTypeCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _roomTypeRepo.GetByNameAsync(request.Name);

            if (existingEntity != null)
            {
                throw new BusinessException($"We're sorry, but it looks like we already have a room type with the name {request.Name}.");
            }

            var entity = new RoomType(request.Name, request.Capacity);
            _roomTypeRepo.Add(entity);
            await _roomTypeRepo.UnitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }

    private class Validator : AbstractValidator<AddRoomTypeCommand>
    {
        public Validator()
        {
            RuleFor(cmd => cmd.Name).NotEmpty();
            RuleFor(cmd => cmd.Capacity).NotEmpty();
        }
    }
}