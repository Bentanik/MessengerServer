﻿using Microsoft.AspNetCore.Http;

namespace MessengerServer.Src.Contracts.Abstractions.EditUserRequests;

public class EditAvatarRequest
{
    public string? FileName { get; set; }
    public IFormFile? CropFileAvatar { get; set; }
    public IFormFile? FullFileAvatar { get; set; }
}
