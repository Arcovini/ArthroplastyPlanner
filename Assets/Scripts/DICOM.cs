using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FellowOakDicom;
using FellowOakDicom.Media;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.Codec;

public class DICOM : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.RawImage rawImage;

    private void Awake()
    {
        // var dir = DicomDirectory.Open(@"C:\Users\Victor Bauer\Downloads\Class-3-malocclusion\Malocclusion\DICOMDIR");

        // foreach(var patientRecord in dir.RootDirectoryRecordCollection)
        // {

        //     foreach(var studyRecord in patientRecord.LowerLevelDirectoryRecordCollection)
        //     {
        //         foreach(var seriesRecord in studyRecord.LowerLevelDirectoryRecordCollection)
        //         {
        //             foreach(var imageRecord in seriesRecord.LowerLevelDirectoryRecordCollection)
        //             {
        //                 var fileId = imageRecord.GetSingleValue<string>(DicomTag.ReferencedFileID);
        //                 GetValues<string>
        //                 Debug.Log(fileId);
        //             }
        //         }   
        //     }
        // }

        // TODO: VERIFICAR TRANSFER SYNTAX (DEFAULT = VRLITTLEENDIAN)

        // try:
        var file = DicomFile.Open(@"C:\Users\Victor Bauer\Downloads\OMBRO\Volume\IMG0150.dcm");
        //var img = file.Dataset.GetDicomItem(DicomTag.PixelData);

        foreach(var i in file.Dataset)
            Debug.Log(i);

        //var dataset = DicomCodecExtensions.Clone(file.Dataset, DicomTransferSyntax.ImplicitVRLittleEndian);

        DicomPixelData pixelData = DicomPixelData.Create(file.Dataset);
        Debug.Log($"{pixelData.Width} {pixelData.Height}");

        // Uses DicomPixelData
        // use try catch for unsupported file formats

        byte[] originalRawBytes = pixelData.GetFrame(0).Data;

        // byte[] modifiedRawBytes = new byte[originalRawBytes.Length / 2];
        // for(int i = 0; i < originalRawBytes.Length; i+=2)
        // {
        //     modifiedRawBytes[i / 2] = (byte)(originalRawBytes[i]);
        // }

        for(int i = 0; i < 10; ++i)
            Debug.Log(originalRawBytes[i]);

        Texture2D tex = new Texture2D(pixelData.Width, pixelData.Height, TextureFormat.RG16, false);
        //tex.LoadRawTextureData(modifiedRawBytes);
        tex.LoadRawTextureData(originalRawBytes);
        tex.Apply();
        
        rawImage.rectTransform.sizeDelta = new Vector2(pixelData.Width, pixelData.Height);
        rawImage.texture = tex;
    }
}