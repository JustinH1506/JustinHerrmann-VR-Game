Shader "Custom/HolographicScanlines"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _ScanlineColor ("Scanline Color", Color) = (1,1,1,0.5)
        _ScanlineSpeed ("Scanline Speed", Range(0, 10)) = 5
        _ScanlineWidth ("Scanline Width", Range(0, 1)) = 0.05
        _GlowStrength ("Glow Strength", Range(0, 2)) = 1
    }
    
    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Opaque" }

        Pass
        {
            Name "HolographicScanlines"
            
             // Make the shader render both sides of the sprite
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Properties from the shader
            sampler2D _MainTex;
            float4 _ScanlineColor;
            float _ScanlineSpeed;
            float _ScanlineWidth;
            float _GlowStrength;

            // Vertex shader
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Fragment shader
            half4 frag(v2f i) : SV_Target
            {
                // Time-based scrolling factor for the scanlines
                float time = _Time.y * _ScanlineSpeed;

                // Generate moving scanlines based on UV.y position
                float scanline = sin(i.uv.y * 20.0 + time * 2.0) * 0.5 + 0.5;
                scanline = smoothstep(_ScanlineWidth - 0.05, _ScanlineWidth, scanline);

                // Sample the main texture (sprite)
                half4 texColor = tex2D(_MainTex, i.uv);

                   if (texColor.a < 0.1)
                {
                    discard;  // Skip fragments with low alpha (transparent)
                }

                // Blend the scanline effect with the base texture color (without ignoring original color)
                half4 finalColor = texColor;

                // Apply the scanline effect by modulating the RGB channels with the scanline color
                finalColor.rgb = lerp(finalColor.rgb, _ScanlineColor.rgb, scanline);

                // Apply glow effect (optional)
                finalColor.rgb += texColor.rgb * _GlowStrength * 0.5;

                // Return the final color, keeping the original alpha from the texture
                finalColor.a = texColor.a;  // Ensure the alpha remains from the texture

                return finalColor;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}

