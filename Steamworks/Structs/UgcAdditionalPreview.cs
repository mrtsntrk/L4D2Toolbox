﻿namespace Steamworks.Data;

public struct UgcAdditionalPreview
{
    internal UgcAdditionalPreview(string urlOrVideoID, string originalFileName, ItemPreviewType itemPreviewType)
    {
        this.UrlOrVideoID = urlOrVideoID;
        this.OriginalFileName = originalFileName;
        this.ItemPreviewType = itemPreviewType;
    }

    public string UrlOrVideoID { get; private set; }
    public string OriginalFileName { get; private set; }
    public ItemPreviewType ItemPreviewType { get; private set; }
}
