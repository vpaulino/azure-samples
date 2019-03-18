using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading;

namespace Azure.Storage.Abstractions
{
    public abstract class StorageProvider
    {
        private readonly StorageSettings settings;

        protected StorageProvider(StorageSettings settings)
        {
            this.settings = settings;
        }

        protected void IsCancelled(CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
                throw new OperationCanceledException();

        }

        protected virtual CloudStorageAccount GetCloudStorageAccount()
        { 
            return CloudStorageAccount.Parse(this.settings.ConnectionString);
        }

    


    }
}