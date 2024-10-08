﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;

using TdmPrototypeDmpSynchroniser.Models;


namespace TdmPrototypeDmpSynchroniser.Services;

public interface IBusService
{
    public Task<Status> CheckBusAsync();

    public Task<Status> CheckBusAsync(string uri);
    
}