Shader "Unlit/HueshiftSprite"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _HueShift ("HueShift", Float) = 0
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment SpriteFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA

			#include "UnityCG.cginc"

			#ifdef UNITY_INSTANCING_ENABLED

				UNITY_INSTANCING_CBUFFER_START(PerDrawSprite)
					// SpriteRenderer.Color while Non-Batched/Instanced.
					fixed4 unity_SpriteRendererColorArray[UNITY_INSTANCED_ARRAY_SIZE];
					// this could be smaller but that's how bit each entry is regardless of type
					float4 unity_SpriteFlipArray[UNITY_INSTANCED_ARRAY_SIZE];
				UNITY_INSTANCING_CBUFFER_END

				#define _RendererColor unity_SpriteRendererColorArray[unity_InstanceID]
				#define _Flip unity_SpriteFlipArray[unity_InstanceID]

			#endif // instancing

			CBUFFER_START(UnityPerDrawSprite)
			#ifndef UNITY_INSTANCING_ENABLED
				fixed4 _RendererColor;
				float4 _Flip;
			#endif
				float _EnableExternalAlpha;
			CBUFFER_END

			// Material Color.
			fixed4 _Color;
			float _HueShift;

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f SpriteVert(appdata_t IN)
			{
				v2f OUT;

				UNITY_SETUP_INSTANCE_ID (IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

			#ifdef UNITY_INSTANCING_ENABLED
				IN.vertex.xy *= _Flip.xy;
			#endif

				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color * _RendererColor;

				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

			#if ETC1_EXTERNAL_ALPHA
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
			#endif

				return color;
			}

			float3 rgb_to_hsv_no_clip(float3 RGB)
			{
				float3 HSV;
			
				float minChannel, maxChannel;
				if (RGB.x > RGB.y) {
					maxChannel = RGB.x;
					minChannel = RGB.y;
				} else {
					maxChannel = RGB.y;
					minChannel = RGB.x;
				}
			
				if (RGB.z > maxChannel) maxChannel = RGB.z;
				if (RGB.z < minChannel) minChannel = RGB.z;
			
				HSV.xy = 0;
				HSV.z = maxChannel;
				float delta = maxChannel - minChannel;             //Delta RGB value
				if (delta != 0) {                    // If gray, leave H  S at zero
					HSV.y = delta / HSV.z;
					float3 delRGB = (HSV.zzz - RGB + 3*delta) / (6.0*delta);

					if      ( RGB.x == HSV.z ) HSV.x = delRGB.z - delRGB.y;
					else if ( RGB.y == HSV.z ) HSV.x = (1.0/3.0) + delRGB.x - delRGB.z;
					else if ( RGB.z == HSV.z ) HSV.x = (2.0/3.0) + delRGB.y - delRGB.x;
				}

				return HSV;
			}
	
			float3 hsv_to_rgb(float3 HSV)
			{
				float3 RGB = HSV.z;
		
				float var_h = HSV.x * 6;
				float var_i = floor(var_h);   // Or ... var_i = floor( var_h )
				float var_1 = HSV.z * (1.0 - HSV.y);
				float var_2 = HSV.z * (1.0 - HSV.y * (var_h-var_i));
				float var_3 = HSV.z * (1.0 - HSV.y * (1-(var_h-var_i)));
				if      (var_i == 0) { RGB = float3(HSV.z, var_3, var_1); }
				else if (var_i == 1) { RGB = float3(var_2, HSV.z, var_1); }
				else if (var_i == 2) { RGB = float3(var_1, HSV.z, var_3); }
				else if (var_i == 3) { RGB = float3(var_1, var_2, HSV.z); }
				else if (var_i == 4) { RGB = float3(var_3, var_1, HSV.z); }
				else                 { RGB = float3(HSV.z, var_1, var_2); }
		
				return RGB;
			}

			fixed4 SpriteFrag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				float3 hsv = rgb_to_hsv_no_clip(c.rgb);
				hsv.x += _HueShift;
				c.rgb = hsv_to_rgb(hsv) * c.a;
				return c;
			}
        ENDCG
        }
    }
}

