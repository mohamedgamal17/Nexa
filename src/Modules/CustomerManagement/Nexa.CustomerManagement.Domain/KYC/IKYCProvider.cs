namespace Nexa.CustomerManagement.Domain.KYC
{
    public interface IKYCProvider
    {
        Task<KYCClient> CreateClientAsync(KYCClientRequest request , CancellationToken cancellationToken = default);
        Task<KYCClient> UpdateClientAsync(string clientId,  KYCClientRequest request , CancellationToken cancellationToken = default);
        Task<KYCDocument> CreateDocumentAsync(KYCDocumentRequest request, CancellationToken cancellationToken = default);
        Task<KYCDocument> UpdateDocumentAsync(string documentId,KYCDocumentRequest request, CancellationToken cancellationToken = default);
        Task<KYCDocument> GetDocumentAsync(string documentId, CancellationToken cancellationToken = default);
        Task DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default);
        Task<KYCDocumentAttachement> UploadDocumentAttachementAsync(string documentId, KYCDocumentAttachmentRequest request, CancellationToken cancellationToken = default);

        Task DeleteDocumentAttachementAsync(string documentId, string attachmentId, CancellationToken cancellationToken = default);
        Task<KYCDocumentAttachement> DownloadDocumentAttachementAsync(string documentId, string attachmentId);
        Task<KYCCheck> CreateCheckAsync(KYCCheckRequest request, CancellationToken cancellationToken = default);
        Task<KYCCheck> GetCheckAsync(string checkId, CancellationToken cancellationToken = default);

        Task<bool> VerifiyWebHookSignature(string signature, string body);
    }
}
