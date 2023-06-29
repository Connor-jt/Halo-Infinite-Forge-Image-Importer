using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;

using System.IO;
using System.Windows.Media.Imaging;

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

        float[] intensity_base = { 0.07176145f, 0.07176145f, 0.07176145f };
        // forge_palette_index[128], color_intensity[100], RGB[3]
        float[,,] intensity_color_list = new float[128,101,3];

        private MainWindow main;

        // adding this incase there is ever a need to scale the pixels spacing aswell
        public ImageArrayifier(double _scale, double x, double y, double z, MainWindow _main){
            scale    = _scale;
            global_X = x;
            global_Y = y;
            global_Z = z;
            main = _main;
        }
        double scale = 1;
        double global_X = 0;
        double global_Y = 0;
        double global_Z = 0;

        

        //  ==========================
        //  == SAMPLE CALL FUNCTION ==
        //  ==========================
        //  void get_pixels_into_nicer_format()
        //  {
        //      var all_pixels = ImageArrayifier.pixel_queue(@"C:\Users\David\Downloads\4854-amogus.png");
        //      for (int i = 0; i < all_pixels.Length; i++)
        //      {
        //          var pixel = all_pixels[i];
        //          // now interface this with the tool to spawn in each object
        //          // note: this could be simplified if you just did this inside of the function that creates the array, opposed to actually creating the array
        //          // pixel.color
        //          // pixel.X
        //          // pxiel.Y
        //          // pixel.Z -- this is 0, unless you create an override
        //      }
        //  }

        public struct mapped_object{
            public int color_index;
            public double X;
            public double Y;
            public double Z;
        }
        /*
        public return_object()
        {
            mapped_object[]? pixels;
            string? output_message;
            int pixel_count;
            float image_accuracy;

        }
        */

        public mapped_object[] pixel_queue(string file_directory, bool build_vertical){
            Bitmap myBitmap = new Bitmap(file_directory);
            Bitmap visualizedBitmap = new Bitmap(file_directory);

            build_intensity_table(); // only needed in intensity consideration

            mapped_object[] pixels = new mapped_object[myBitmap.Width*myBitmap.Height];
            int curr_pixel = 0;
            for (int x=0; x < myBitmap.Width; x++){
                for (int y = 0; y < myBitmap.Height; y++){

                    //int color_index = get_index_of_closest_color(myBitmap.GetPixel(x, y));
                    //visualizedBitmap.SetPixel(x, y, color_by_list_index(color_index));
                    KeyValuePair<int, int> color_index = get_index_and_intensity_of_closest_color(myBitmap.GetPixel(x, y));
                    visualizedBitmap.SetPixel(x, y, color_by_intensity_list_index(color_index.Key, color_index.Value));


                    if (build_vertical) pixels[curr_pixel] = new mapped_object { color_index = color_index.Key, X = global_X + (x * scale), Y = global_Y + (y * scale), Z = global_Z };
                    else                pixels[curr_pixel] = new mapped_object { color_index = color_index.Key, X = global_X + (x*scale), Y = global_Y + (y*scale), Z = global_Z };
                    curr_pixel++;
                }
            }
            // show the images
            main.og_image.Source   = BitmapToImageSource(myBitmap);
            main.demo_image.Source = BitmapToImageSource(visualizedBitmap);
            // return array
            return pixels;
        }

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

        float color_as_float(byte color){
            return (float)color / 255;
        }
        Color color_by_list_index(int index){
            return Color.FromArgb((byte)(color_list[index,0] * 255), (byte)(color_list[index,1] * 255), (byte)(color_list[index,2] * 255));
        }


        void build_intensity_table(){
            for (int i = 0; i < color_list.Length / 3; i++){ // my c sharp brothers in christ, why does length tell me the length OF THE WHOLE THING
                float r = color_list[i, 0];
                float b = color_list[i, 1];
                float g = color_list[i, 2];

                for (int intensity = 0; intensity <= 100; intensity++){
                    float intensity_f = (float)intensity / 100;

                    intensity_color_list[i,intensity,0] = intensity_base[0] + (r * intensity_f);
                    intensity_color_list[i,intensity,1] = intensity_base[1] + (g * intensity_f);
                    intensity_color_list[i,intensity,2] = intensity_base[2] + (b * intensity_f);
                }
            }
        }
        KeyValuePair<int, int> get_index_and_intensity_of_closest_color(Color og_color){
            float r = color_as_float(og_color.R);
            float b = color_as_float(og_color.G);
            float g = color_as_float(og_color.B);

            int closest_palette_index = 0;
            int closest_intensity_index = 0;
            float? closest_match_distance = null;

            for (int i = 0; i < color_list.Length / 3; i++){ // my c sharp brothers in christ, why does length tell me the length OF THE WHOLE THING

                for (int intensity = 0; intensity <= 100; intensity++){
                    float i_r = intensity_color_list[i, intensity, 0];
                    float i_b = intensity_color_list[i, intensity, 1];
                    float i_g = intensity_color_list[i, intensity, 2];

                    float distance = Math.Abs(r - i_r) + Math.Abs(g - i_g) + Math.Abs(b - i_b);
                    if (closest_match_distance == null || distance < closest_match_distance){
                        closest_palette_index = i;
                        closest_intensity_index = intensity;
                        closest_match_distance = distance;
                    }
                }
            }

            return new KeyValuePair<int, int> (closest_palette_index, closest_intensity_index);

        }
        Color color_by_intensity_list_index(int index, int intensity){
            return Color.FromArgb((byte)(intensity_color_list[index,intensity,0] * 255), (byte)(intensity_color_list[index,intensity,1] * 255), (byte)(intensity_color_list[index,intensity,2] * 255));
        }




        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
