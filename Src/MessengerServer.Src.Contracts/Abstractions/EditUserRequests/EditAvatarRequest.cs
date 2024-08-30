using Microsoft.AspNetCore.Http;

namespace MessengerServer.Src.Contracts.Abstractions.EditUserRequests;

public class EditCoverPhotoRequest
{
    public IFormFile? CropFileCoverPhoto { get; set; }
    public IFormFile? FullFileCoverPhoto { get; set; }
}
