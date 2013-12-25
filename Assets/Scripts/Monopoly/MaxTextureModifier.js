/*#pragma strict

var Textures: Texture[];
function Awake() {
	for(var i = 0; i < Textures.length; i++){
		var texturePath = AssetDatabase.GetAssetPath(Textures[i]);
		var AsImp : TextureImporter = AssetImporter.GetAtPath(texturePath);
		AsImp.mipmapEnabled = true;
		AsImp.isReadable = true;
		AsImp.maxTextureSize = 5140;
		AssetDatabase.ImportAsset(texturePath);
	}	
}
function Start () {

}

function Update () {

}*/