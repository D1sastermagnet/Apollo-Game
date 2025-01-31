﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu()]
public class TextureData : UpdatableObject
{
    const int textureSize = 512;
    const TextureFormat textureFormat = TextureFormat.RGB565;
    public Layer[] layers;
    public float curveStrength = 2f;
    public float curveFalloff;

    float savedMinHeight;
    float savedMaxHeight;
    public void ApplyToMaterial(Material material)
    {
        material.SetInt("layerCount", layers.Length);
        material.SetColorArray("baseColours", layers.Select(x => x.tint).ToArray());
        material.SetFloatArray("baseColourStrength", layers.Select(x => x.tintStrength).ToArray());
        material.SetFloatArray("baseStartHeights", layers.Select(x => x.startHeight).ToArray());
        material.SetFloatArray("baseBlends", layers.Select(x => x.blendStrength).ToArray());
        material.SetFloatArray("baseTextureScales", layers.Select(x => x.textureScale).ToArray());
        material.SetFloatArray("baseBrightnessCorrection", layers.Select(x => x.brightnessCorrection).ToArray());
        material.SetFloat("curveStrength", curveStrength);
        material.SetFloat("curveFalloff", curveFalloff);

        Texture2DArray textureArray = GenerateTextureArray(layers.Select(x => x.texture).ToArray());
        material.SetTexture("baseTextures", textureArray);

        UpdateMeshHeights(material, savedMinHeight, savedMaxHeight);
    }

    public void ApplyToObjectMaterial(Material material){
        material.SetFloat("curveStrength", curveStrength);
        material.SetFloat("curveFalloff", curveFalloff);
    }

    public void ApplyToAllObjectMaterials(){
        Object[] objectMaterials = Resources.LoadAll("Object Materials/Unity 5");

        for(int i = 0; i < objectMaterials.Length; i++){
            ApplyToObjectMaterial((Material)objectMaterials[i]);
        }
    }

    public void UpdateMeshHeights(Material material, float minHeight, float maxHeight){
        savedMinHeight = minHeight;
        savedMaxHeight = maxHeight;

        material.SetFloat("minHeight", minHeight);
        material.SetFloat("maxHeight", maxHeight);
    }

    Texture2DArray GenerateTextureArray(Texture2D[] textures){
        Texture2DArray textureArray = new Texture2DArray(textureSize, textureSize, textures.Length, textureFormat, true);
        for(int i = 0; i < textures.Length; i++){
            textureArray.SetPixels(textures[i].GetPixels(), i);
        }

        textureArray.Apply();
        return textureArray;
    }

    [System.Serializable]
    public class Layer{
        public Texture2D texture;
        public Color tint;
        [Range(0,1)]
        public float tintStrength;
        [Range(0,1)]
        public float startHeight;
        [Range(0,1)]
        public float blendStrength;
        public float textureScale;
        [Range(-1,1)]
        public float brightnessCorrection = 0f;
    }
}
