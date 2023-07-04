using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;

using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Ribbon.Primitives;
using System.Reflection.Metadata.Ecma335;

namespace imaginator_halothousand.code_stuff
{
    internal class ImageArrayifier
    {
        float[,] color_list = {
            {1f,          1f,          1f           },
            {0.7969165f,  0.7969165f,  0.79691654f  },
            {0.6120656f,  0.6120656f,  0.6120656f   },
            {0.4590799f,  0.4590799f,  0.4590799f   },
            {0.325037f,   0.325037f,   0.32503697f  },
            {0.2195197f,  0.2195197f,  0.21951972f  },
            {0.1332085f,  0.1332085f,  0.13320851f  },
            {0.07176145f, 0.07176145f, 0.07176145f  },
            {0.02899119f, 0.02899119f, 0.028991187f },
            {0.006584957f,0.006584957f,0.006584957f },
            {0f,          0f,          0f           },
            {0.6120656f,  0.02899119f, 0.13320851f  },
            {0.5479935f,  0.009021492f,0.09151835f  },
            {0.7226725f,  0.01793643f, 0.05112205f  },
            {0.7226725f,  0.00369724f, 0.041451894f },
            {0.5604991f,  0.009021492f,0.038472746f },
            {0.7817507f,  0.01188133f, 0.005028203f },
            {1f,          0f,          0f           },
            {0.4534565f,  0.01188133f, 0.011881334f },
            {0.3344578f,  0.002585826f,0.009021492f },
            {0.2590273f,  0.000367136f,0.000367136f },
            {0.2010957f,  0.002585826f,0.003302703f },
            {0.325037f,   0.1332085f,  1f           },
            {0.6120656f,  0.1332085f,  0.6120656f   },
            {0.325037f,   0.1332085f,  0.32503697f  },
            {0.325037f,   0f,          0.32503697f  },
            {0.1332085f,  0f,          0.13320851f  },
            {0.06772459f, 0.0563741f,  0.07592612f  },
            {0.6120656f,  0.325037f,   0.6120656f   },
            {0.6120656f,  0.325037f,   1f           },
            {0.6120656f,  0.1332085f,  1f           },
            {0.325037f,   0.02899119f, 0.6120656f   },
            {0.325037f,   0f,          0.6120656f   },
            {0.325037f,   0f,          1f           },
            {0.1332085f,  0.02899119f, 0.32503697f  },
            {0.02899119f, 0f,          0.13320851f  },
            {0.6120656f,  0.6120656f,  1f           },
            {0.08690125f, 0.1011452f,  0.3940831f   },
            {0.02899119f, 0f,          0.32503697f  },
            {0.1332085f,  0.02899119f, 1f           },
            {0.1332085f,  0f,          0.6120656f   },
            {0f,          0f,          1f           },
            {0.04614884f, 0f,          1f           },
            {0.02899119f, 0f,          0.6120656f   },
            {0.000492504f,0.000107187f,0.52344316f  },
            {0.1332085f,  0.325037f,   1f           },
            {0f,          0.06003161f, 0.41514808f  },
            {0f,          0.1636407f,  0.4877652f   },
            {0.02899119f, 0.1332085f,  0.32503697f  },
            {0f,          0.02899119f, 0.32503697f  },
            {0.006040594f,0.006040594f,0.16364068f  },
            {0.008373118f,0.01793643f, 0.08021926f  },
            {0.325037f,   0.325037f,   1f           },
            {0.1332085f,  0.1332085f,  0.6120656f   },
            {0.1332085f,  0.1332085f,  1f           },
            {0.4259053f,  0.6940805f,  0.79691654f  },
            {0.1332085f,  0.325037f,   0.32503697f  },
            {0f,          0.1332085f,  0.13320851f  },
            {0.325037f,   1f,          1f           },
            {0.1332085f,  1f,          0.6120656f   },
            {0f,          1f,          1f           },
            {0f,          0.6120656f,  1f           },
            {0f,          0.325037f,   0.32503697f  },
            {0.325037f,   0.6120656f,  0.6120656f   },
            {0.325037f,   1f,          0.32503697f  },
            {2.3328E-05f, 1f,          0.6120656f   },
            {0.02899119f, 0.6120656f,  0.32503697f  },
            {0.02899119f, 0.6120656f,  0.13320851f  },
            {0.02899119f, 0.325037f,   0.13320851f  },
            {0.1332085f,  0.325037f,   0.13320851f  },
            {0.325037f,   0.6120656f,  0.32503697f  },
            {0.1087112f,  0.5479935f,  0.009021492f },
            {0f,          1f,          0f           },
            {0.02899119f, 0.6120656f,  0.028991187f },
            {0f,          0.325037f,   0.028991187f },
            {0.6120656f,  1f,          0.13320851f  },
            {0.7445301f,  1f,          0f           },
            {0.325037f,   0.6120656f,  0.028991187f },
            {0.325037f,   0.6120656f,  0.13320851f  },
            {0.2715774f,  0.47617728f, 0f           },
            {0.09151835f, 0.2271365f,  5.69218E-05f },
            {0.1332085f,  0.1332085f,  0.028991187f },
            {0.05459228f, 0.05284163f, 0.03025652f  },
            {0.02899119f, 0.028991187f,0f           },
            {0.01991784f, 0.04777575f, 0.01266372f  },
            {0f,          0.02899119f, 0.028991187f },
            {0.02899119f, 0.1332085f,  0.13320851f  },
            {1f,          1f,          0.6120656f   },
            {1f,          1f,          0.32503697f  },
            {1f,          1f,          0.13320851f  },
            {0.948965f,   0.7969165f,  0.13320851f  },
            {1f,          1f,          0f           },
            {0.6120656f,  0.6120656f,  0.028991187f },
            {0.325037f,   0.325037f,   0.028991187f },
            {0.4136255f,  0.2639139f,  0.03844675f  },
            {1f,          0.6120656f,  0f           },
            {1f,          0.325037f,   0.13320851f  },
            {1f,          0.325037f,   0.028991187f },
            {0.8912621f,  0.23882799f, 0f           },
            {0.8993845f,  0.1904629f,  0.019917838f },
            {1f,          0.1332085f,  0.028991187f },
            {1f,          0.13320851f, 0f           },
            {0.9157501f,  0.9157501f,  0.72267246f  },
            {0.8751376f,  0.8277258f,  0.68002033f  },
            {0.6523701f,  0.4647411f,  0.26735806f  },
            {1f,          0.6120656f,  0.32503697f  },
            {0.6120656f,  0.1332085f,  0.028991187f },
            {0.4819523f,  0.04943346f, 0.001686915f },
            {0.5174013f,  0.4819523f,  0.14799802f  },
            {0.325037f,   0.1332085f,  0.028991187f },
            {0.1332085f,  0.02899119f, 0.028991187f },
            {0.1332085f,  0.028991187f,0f           },
            {1f,          0.6120656f,  0.6120656f   },
            {1f,          0.325037f,   0.32503697f  },
            {1f,          0.1332085f,  0.13320851f  },
            {1f,          0.325037f,   0.6120656f   },
            {1f,          0.325037f,   1f           },
            {1f,          0.1332085f,  0.32503697f  },
            {1f,          0f,          1f           },
            {1f,          0.02899119f, 0.32503697f  },
            {1f,          0f,          0.13320851f  },
            {0.6120656f,  0.02899119f, 1f           },
            {0.004116177f,0.9075472f,  0.8591736f   },
            {0.2348953f,  0.9913929f,  0.000107187f },
            {0.9743002f,  0.9913929f,  0.000107187f },
            {0.9743002f,  0.2120444f,  0.000107187f },
            {0.9743002f,  0.07382777f, 0.9743002f   },
            {0.1087112f,  0.00120174f, 0.85125166f  },
        };

        // //////////////// //
        // SWATCHES COLORS //
        // ////////////// //


        // SWATCH : 'Snowman Snow' //
        // float[] intensity_base = { 0.7969165f, 0.7969165f, 0.79691654f };

        // SWATCH : 'Brushed' //
        // float[] intensity_base = { 0.6120656f, 0.6120656f, 0.6120656f };

        // SWATCH : 'Paint' //
        float[] intensity_base = { 0.2195197f, 0.2195197f, 0.21951972f };

        // SWATCH : 'Plastic Techsuit' //
        // float[] intensity_base = { 0.07176145f, 0.07176145f, 0.07176145f };

        // SWATCH : 'Enamal Smooth' //
        // float[] intensity_base = { 0.48627450f, 0.96078431f, 0.03921568f };

        // SWATCH : 'Copper Metal Scratched' //
        // float[] intensity_base = { 0.54089517f, 0.1732085f, 0.0f };

        // float[] intensity_base = { 0.5f, 0.5f, 0.5f };
        // SWATCH : '' //
        //float[] intensity_base = { 0.0f, 0.0f, 0.0f };


        // forge_palette_index[128], color_intensity[100], RGB[3]
        float[,,] intensity_color_list = new float[128,101,3];


        // adding this incase there is ever a need to scale the pixels spacing aswell


        public struct mapped_object{
            public int color_index;
            public int intensity_index;
            public double X;
            public double Y;
        }
        
        public class return_object{
            public Bitmap? source_img;
            public Bitmap? visualized_img;

            public List<mapped_object> pixels;
            public string? output_message;
            public int pixel_count;
            public int visible_pixel_count;
            public double image_accuracy = 0.0f;
            

        }

        public return_object result = new ();


        public return_object pixel_queue(string file_directory){

            result.source_img = new Bitmap(file_directory);
            result.visualized_img = new Bitmap(file_directory);

            build_intensity_table(); // only needed in intensity consideration

            result.pixel_count = result.source_img.Width * result.source_img.Height;
            result.pixels = new List<mapped_object>();
            result.visible_pixel_count = result.pixel_count;

            int curr_pixel = 0;
            for (int x=0; x < result.source_img.Width; x++){
                for (int y = 0; y < result.source_img.Height; y++){
                    // check if pixel is transparent, if so skip it
                    Color pixel = result.source_img.GetPixel(x, y);
                    if (pixel.A == 0) {
                        curr_pixel++;
                        result.visible_pixel_count--;
                        result.image_accuracy += 1.0 / result.pixel_count;
                        result.visualized_img.SetPixel(x, y, Color.FromArgb(0xFF, 0x00, 0x00));
                        continue;
                    }

                    //int color_index = get_index_of_closest_color(myBitmap.GetPixel(x, y));
                    //visualizedBitmap.SetPixel(x, y, color_by_list_index(color_index));
                    KeyValuePair<int, int> color_index = get_index_and_intensity_of_closest_color(pixel);
                    result.visualized_img.SetPixel(x, y, color_by_intensity_list_index(color_index.Key, color_index.Value));


                    result.pixels.Add(new mapped_object { color_index = color_index.Key, intensity_index = color_index.Value, X = x, Y = y });
                    curr_pixel++;
                }
            }
            // show the images
            //main.og_image.Source   = BitmapToImageSource(myBitmap);
            //main.demo_image.Source = BitmapToImageSource(visualizedBitmap);
            // return array
            return result;
        }
        #region PALETTE_INDEX_COLORS
        int get_index_of_closest_color(Color og_color){
            float r = color_as_float(og_color.R);
            float b = color_as_float(og_color.G);
            float g = color_as_float(og_color.B);

            int closest_match_index = 0;
            float? closest_match_distance = null;

            for (int i = 0; i < color_list.Length/3; i++){ // my c sharp brothers in christ, why does length tell me the length OF THE WHOLE THING
                float i_r = color_list[i,0];
                float i_b = color_list[i,1];
                float i_g = color_list[i,2];

                float distance = Math.Abs(r - i_r) + Math.Abs(g - i_g) + Math.Abs(b - i_b);
                if (closest_match_distance == null || distance < closest_match_distance){
                    closest_match_index = i;
                    closest_match_distance = distance;
                }
            }
            return closest_match_index;
        }
        Color color_by_list_index(int index){
            return Color.FromArgb((byte)(color_list[index, 0] * 255), (byte)(color_list[index, 1] * 255), (byte)(color_list[index, 2] * 255));
        }
        #endregion
        #region PALETTE_INTENSITY_COLORS
        void build_intensity_table(){
            for (int i = 0; i < color_list.Length / 3; i++){ // my c sharp brothers in christ, why does length tell me the length OF THE WHOLE THING
                float r = color_list[i, 0];
                float g = color_list[i, 1];
                float b = color_list[i, 2];

                for (int intensity = 0; intensity <= 100; intensity++){
                    float intensity_f = (float)intensity / 100;


                    // interpolate between base color and new
                    // 

                    intensity_color_list[i,intensity,0] = interpolate_colors(intensity_base[0], r, intensity_f);
                    intensity_color_list[i,intensity,1] = interpolate_colors(intensity_base[1], g, intensity_f);
                    intensity_color_list[i,intensity,2] = interpolate_colors(intensity_base[2], b, intensity_f);

                    if (intensity_color_list[i, intensity, 0] > 1.0f || intensity_color_list[i, intensity, 1] > 1.0f || intensity_color_list[i, intensity, 2] > 1.0f)
                    {
                        continue;
                    }
                    if (intensity_color_list[i, intensity, 0] < 0.0f || intensity_color_list[i, intensity, 1] < 0.0f || intensity_color_list[i, intensity, 2] < 0.0f)
                    {
                        continue;
                    }
                }
            }}
        float interpolate_colors(float A, float B, float factor){
            return A - ((A - B) * factor);
        }



        KeyValuePair<int, int> get_index_and_intensity_of_closest_color(Color og_color){
            float r = color_as_float(og_color.R);
            float g = color_as_float(og_color.G);
            float b = color_as_float(og_color.B);

            int closest_palette_index = 0;
            int closest_intensity_index = 0;
            float? closest_match_distance = null;

            for (int i = 0; i < color_list.Length / 3; i++){ // my c sharp brothers in christ, why does length tell me the length OF THE WHOLE THING

                for (int intensity = 0; intensity <= 100; intensity++){
                    float i_r = intensity_color_list[i, intensity, 0];
                    float i_g = intensity_color_list[i, intensity, 1];
                    float i_b = intensity_color_list[i, intensity, 2];

                    float distance = float_rb_dist(r, i_r) + float_rb_dist(g, i_g) + float_rb_dist(b, i_b);
                    if (closest_match_distance == null || distance < closest_match_distance){
                        closest_palette_index = i;
                        closest_intensity_index = intensity;
                        closest_match_distance = distance;
                    }
                }
            }
            float _i_r = intensity_color_list[closest_palette_index, closest_intensity_index, 0];
            float _i_g = intensity_color_list[closest_palette_index, closest_intensity_index, 1];
            float _i_b = intensity_color_list[closest_palette_index, closest_intensity_index, 2];
            // calculate accuracy
            result.image_accuracy += (1.0 - ((double)closest_match_distance / 3.0)) / result.pixel_count;
            return new KeyValuePair<int, int> (closest_palette_index, closest_intensity_index);

        }
        float float_rb_dist(float A, float B){
            float linear_dist = Math.Abs(A - B);
            //if (linear_dist > 0.5f){ // thinking about this now, it seems this is actually a bad idea
            //    return 1.0f - linear_dist;
            //} 
            return linear_dist;
        }
        Color color_by_intensity_list_index(int index, int intensity){
            return Color.FromArgb((byte)(intensity_color_list[index,intensity,0] * 255), (byte)(intensity_color_list[index,intensity,1] * 255), (byte)(intensity_color_list[index,intensity,2] * 255));
        }
        #endregion

        float color_as_float(byte color){
            return (float)color / 255f;
        }

    }
}
