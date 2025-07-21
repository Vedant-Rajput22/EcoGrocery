using MediatR;

namespace Application.Features.Products.Commands;

public record FileUploadVm(byte[] Bytes, string FileName, string ContentType);
public record UploadProductImagesCommand(
    Guid ProductId,
    IReadOnlyList<FileUploadVm> Files) : IRequest<Unit>;
