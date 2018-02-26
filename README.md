# GDI+ Extensions
[![license](https://img.shields.io/github/license/mashape/apistatus.svg?style=flat-square)]()
[![Build Status](https://travis-ci.org/zavolokas/GdiExtensions.svg?branch=master)](https://travis-ci.org/zavolokas/GdiExtensions)

Contains extension methods to deal with `Image` classes.

>  PM> Install-Package Zavolokas.GdiExtensions -Version 1.0.0

## Contents
1. [Scaling](#scaling)
2. [Opacity](#opacity)
3. [Channels copy](#channels-copy)

## Scaling

Clones an original image to a new one with changed size.

```csharp
using (var bitmap = new Bitmap(pathToImageFile))
using (var scaled = bitmap.CloneWithScaleTo(300, 80))
{
    scaled
        .SaveTo(resultPath, ImageFormat.Png)
        .ShowFile();
}
```
| Input image | Result |
| ----------- | ------ |
| ![input1]   | ![scalingOutput1]|

## Opacity 
Clones an original image to a new one with the opacity set to the specidied.
```csharp
using (var image = new Bitmap(pathToImageFile))
using (var semiTransparent = image.CloneWithOpacity(0.3f))
{
    semiTransparent
        .SaveTo(resultPath, ImageFormat.Png)
        .ShowFile();
}
```
| Input image | Result |
| ----------- | ------ |
| ![input1]   | ![opacityOutput]|


## Channels copy
Replaces channel values from a source RGBA image.

> Note:
> The dest and source images should be of the same size.

```csharp
using (var source = new Bitmap(pathToSrcImage))
using (var dest = new Bitmap(pathToDestImage))
{
    const int dstChannelIndex = 2;
    const int srcChannelIndex = 3;

    dest.CopyChannel(dstChannelIndex, source, srcChannelIndex)
        .SaveTo(resultPath, ImageFormat.Png)
        .ShowFile();
}
```
| Dest |Src|
| -  |-|
| ![input1] |![input2]|

| Replace Red with Alpha |Replace Green with Alpha | Replace Blue with Alpha |
| -  |-| - |
| ![channelsOutputAR] |![channelsOutputAG]| ![channelsOutputAB]|

  
[input1]: Images/t023.jpg "Input image" 
[input2]: Images/m023.png "Input image"
[scalingOutput1]: Images/scaling_out.png "Scaled image"
[opacityOutput]: Images/opacity_out.png "Transparent image"
[channelsOutputAR]: Images/channels_ar_out.png "Mixed channel image"
[channelsOutputAG]: Images/channels_ag_out.png "Mixed channel image"
[channelsOutputAB]: Images/channels_out.png "Mixed channel image"
