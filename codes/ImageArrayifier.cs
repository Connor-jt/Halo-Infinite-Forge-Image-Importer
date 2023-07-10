using Accessibility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.AccessControl;
using System.Windows.Media.Media3D;

namespace imaginator_halothousand.code_stuff{
    internal class ImageArrayifier{


        static float[,] color_list = {                 // recorded ingame colors
            {1f,          1f,          1f           }, // {186,186,186},
            {0.7969165f,  0.7969165f,  0.79691654f  }, // {173,173,173},
            {0.6120656f,  0.6120656f,  0.6120656f   }, // {158,158,158},
            {0.4590799f,  0.4590799f,  0.4590799f   }, // {145,145,145},
            {0.325037f,   0.325037f,   0.32503697f  }, // {127,127,127},
            {0.2195197f,  0.2195197f,  0.21951972f  }, // {110,110,110},
            {0.1332085f,  0.1332085f,  0.13320851f  }, // { 90, 90, 90},
            {0.07176145f, 0.07176145f, 0.07176145f  }, // { 68, 68, 68},

            {0.02899119f, 0.02899119f, 0.028991187f }, // { 43, 43, 43},
            {0.006584957f,0.006584957f,0.006584957f }, // { 19, 19, 19},
            {0f,          0f,          0f           }, // {  3,  3,  3},
            {0.6120656f,  0.02899119f, 0.13320851f  }, // {180, 39, 89},
            {0.5479935f,  0.009021492f,0.09151835f  }, // {176, 20, 76},
            {0.7226725f,  0.01793643f, 0.05112205f  }, // {193, 29, 55},
            {0.7226725f,  0.00369724f, 0.041451894f }, // {195, 10, 49},
            {0.5604991f,  0.009021492f,0.038472746f }, // {179, 21, 48},

            {0.7817507f,  0.01188133f, 0.005028203f }, // {199, 22, 12},
            {1f,          0f,          0f           }, // {216,  1,  1},
            {0.4534565f,  0.01188133f, 0.011881334f }, // {166, 24, 24},
            {0.3344578f,  0.002585826f,0.009021492f }, // {148,  9, 22},
            {0.2590273f,  0.000367136f,0.000367136f }, // {137,  3,  3},
            {0.2010957f,  0.002585826f,0.003302703f }, // {125, 11, 11},
            {0.325037f,   0.1332085f,  1f           }, // {130, 86,218},
            {0.6120656f,  0.1332085f,  0.6120656f   }, // {172, 85,175},

            {0.325037f,   0.1332085f,  0.32503697f  }, // {133, 87,136},
            {0.325037f,   0f,          0.32503697f  }, // {143,  1,147},
            {0.1332085f,  0f,          0.13320851f  }, // {102,  1,103},
            {0.06772459f, 0.0563741f,  0.07592612f  }, // { 66, 61, 70},
            {0.6120656f,  0.325037f,   0.6120656f   }, // {165,124,166},
            {0.6120656f,  0.325037f,   1f           }, // {163,122,206},
            {0.6120656f,  0.1332085f,  1f           }, // {169, 84,214},
            {0.325037f,   0.02899119f, 0.6120656f   }, // {136, 41,184},

            {0.325037f,   0f,          0.6120656f   }, // {140,  1,189},
            {0.325037f,   0f,          1f           }, // {135,  1,227},
            {0.1332085f,  0.02899119f, 0.32503697f  }, // { 93, 43,149},
            {0.02899119f, 0f,          0.13320851f  }, // { 49,  2,112},
            {0.6120656f,  0.6120656f,  1f           }, // {156,158,197},
            {0.08690125f, 0.1011452f,  0.3940831f   }, // { 72, 80,154},
            {0.02899119f, 0f,          0.32503697f  }, // { 47,  4,164},
            {0.1332085f,  0.02899119f, 1f           }, // { 89, 41,228},

            {0.1332085f,  0f,          0.6120656f   }, // { 95,  1,197},
            {0f,          0f,          1f           }, // {  1,  1,237},
            {0.04614884f, 0f,          1f           }, // { 54,  1,235},
            {0.02899119f, 0f,          0.6120656f   }, // { 44,  1,204},
            {0.000492504f,0.000107187f,0.52344316f  }, // {  1,  1,199},
            {0.1332085f,  0.325037f,   1f           }, // { 81,126,210},
            {0f,          0.06003161f, 0.41514808f  }, // {  1, 64,168},
            {0f,          0.1636407f,  0.4877652f   }, // {  1,100,168},

            {0.02899119f, 0.1332085f,  0.32503697f  }, // { 39, 90,141},
            {0f,          0.02899119f, 0.32503697f  }, // {  1, 45,157},
            {0.006040594f,0.006040594f,0.16364068f  }, // { 20, 20,125},
            {0.008373118f,0.01793643f, 0.08021926f  }, // { 22, 35, 82},
            {0.325037f,   0.325037f,   1f           }, // {125,125,209},
            {0.1332085f,  0.1332085f,  0.6120656f   }, // { 88, 88,182},
            {0.1332085f,  0.1332085f,  1f           }, // { 87, 87,221},
            {0.4259053f,  0.6940805f,  0.79691654f  }, // {133,168,181},

            {0.1332085f,  0.325037f,   0.32503697f  }, // { 82,129,130},
            {0f,          0.1332085f,  0.13320851f  }, // {  1, 92, 92},
            {0.325037f,   1f,          1f           }, // {112,190,192},
            {0.1332085f,  1f,          0.6120656f   }, // { 73,192,154},
            {0f,          1f,          1f           }, // {  1,192,193},
            {0f,          0.6120656f,  1f           }, // {  1,161,202},
            {0f,          0.325037f,   0.32503697f  }, // {  1,131,131},
            {0.325037f,   0.6120656f,  0.6120656f   }, // {119,160,160},

            {0.325037f,   1f,          0.32503697f  }, // {112,191,116},
            {2.3328E-05f, 1f,          0.6120656f   }, // {  1,193,155},
            {0.02899119f, 0.6120656f,  0.32503697f  }, // { 33,163,124},
            {0.02899119f, 0.6120656f,  0.13320851f  }, // { 33,163, 80},
            {0.02899119f, 0.325037f,   0.13320851f  }, // { 36,130, 86},
            {0.1332085f,  0.325037f,   0.13320851f  }, // { 83,129, 83},
            {0.325037f,   0.6120656f,  0.32503697f  }, // {121,162,121},
            {0.1087112f,  0.5479935f,  0.009021492f }, // { 72,156, 16},

            {0f,          1f,          0f           }, // {  1,193,  1},
            {0.02899119f, 0.6120656f,  0.028991187f }, // { 34,163, 34},
            {0f,          0.325037f,   0.028991187f }, // {  1,132, 38},
            {0.6120656f,  1f,          0.13320851f  }, // {151,190, 74},
            {0.7445301f,  1f,          0f           }, // {165,190,  1},
            {0.325037f,   0.6120656f,  0.028991187f }, // {120,162, 34},
            {0.325037f,   0.6120656f,  0.13320851f  }, // {120,162, 79},
            {0.2715774f,  0.47617728f, 0f           }, // {113,149,  1},

            {0.09151835f, 0.2271365f,  5.69218E-05f }, // { 70,113,  1},
            {0.1332085f,  0.1332085f,  0.028991187f }, // { 90, 90, 40},
            {0.05459228f, 0.05284163f, 0.03025652f  }, // { 59, 59, 44},
            {0.02899119f, 0.028991187f,0f           }, // { 43, 43,  1},
            {0.01991784f, 0.04777575f, 0.01266372f  }, // { 33, 56, 26},
            {0f,          0.02899119f, 0.028991187f }, // {  1, 45, 45},
            {0.02899119f, 0.1332085f,  0.13320851f  }, // { 39, 91, 91},
            {1f,          1f,          0.6120656f   }, // {188,188,151},

            {1f,          1f,          0.32503697f  }, // {188,188,113},
            {1f,          1f,          0.13320851f  }, // {189,189, 73},
            {0.948965f,   0.7969165f,  0.13320851f  }, // {185,175, 76},
            {1f,          1f,          0f           }, // {189,189,  1},
            {0.6120656f,  0.6120656f,  0.028991187f }, // {161,161, 34},
            {0.325037f,   0.325037f,   0.028991187f }, // {128,128, 37},
            {0.4136255f,  0.2639139f,  0.03844675f  }, // {144,118, 44},
            {1f,          0.6120656f,  0f           }, // {194,158,  1},

            {1f,          0.325037f,   0.13320851f  }, // {201,122, 80},
            {1f,          0.325037f,   0.028991187f }, // {202,122, 35},
            {0.8912621f,  0.23882799f, 0f           }, // {196,109,  1},
            {0.8993845f,  0.1904629f,  0.019917838f }, // {199, 98, 30},
            {1f,          0.1332085f,  0.028991187f }, // {210, 83, 36},
            {1f,          0.13320851f, 0f           }, // {210, 83,  1},
            {0.9157501f,  0.9157501f,  0.72267246f  }, // {183,183,165},
            {0.8751376f,  0.8277258f,  0.68002033f  }, // {176,176,162},

            {0.6523701f,  0.4647411f,  0.26735806f  }, // {166,144,112},
            {1f,          0.6120656f,  0.32503697f  }, // {193,157,119},
            {0.6120656f,  0.1332085f,  0.028991187f }, // {174, 86, 38},
            {0.4819523f,  0.04943346f, 0.001686915f }, // {165, 54,  5},
            {0.5174013f,  0.4819523f,  0.14799802f  }, // {151,148, 85},
            {0.325037f,   0.1332085f,  0.028991187f }, // {135, 89, 40},
            {0.1332085f,  0.02899119f, 0.028991187f }, // { 98, 44, 44},
            {0.1332085f,  0.028991187f,0f           }, // { 98,  4,  1},

            {1f,          0.6120656f,  0.6120656f   }, // {193,156,156},
            {1f,          0.325037f,   0.32503697f  }, // {202,122,122},
            {1f,          0.1332085f,  0.13320851f  }, // {209, 83, 83},
            {1f,          0.325037f,   0.6120656f   }, // {200,122,164},
            {1f,          0.325037f,   1f           }, // {199,121,202},
            {1f,          0.1332085f,  0.32503697f  }, // {208, 82,128},
            {1f,          0f,          1f           }, // {212,  1,212},
            {1f,          0.02899119f, 0.32503697f  }, // {214, 37,132},

            {1f,          0f,          0.13320851f  }, // {216,  1, 86},
            {0.6120656f,  0.02899119f, 1f           }, // {174, 38,220},
            {0.004116177f,0.9075472f,  0.8591736f   }, // {  7,186,185},
            {0.2348953f,  0.9913929f,  0.000107187f }, // { 98,193,  1},
            {0.9743002f,  0.9913929f,  0.000107187f }, // {185,189,  1},
            {0.9743002f,  0.2120444f,  0.000107187f }, // {205,103,  1},
            {0.9743002f,  0.07382777f, 0.9743002f   }, // {207, 61,207},
            {0.1087112f,  0.00120174f, 0.85125166f  }, // { 86,  4,224},
        };
        static byte[,] observed_color_list = {
            {186,186,186},{173,173,173},{158,158,158},{145,145,145},{127,127,127},{110,110,110},{ 90, 90, 90},{ 68, 68, 68},
            { 43, 43, 43},{ 19, 19, 19},{  3,  3,  3},{180, 39, 89},{176, 20, 76},{193, 29, 55},{195, 10, 49},{179, 21, 48},
            {199, 22, 12},{216,  1,  1},{166, 24, 24},{148,  9, 22},{137,  3,  3},{125, 11, 11},{130, 86,218},{172, 85,175},
            {133, 87,136},{143,  1,147},{102,  1,103},{ 66, 61, 70},{165,124,166},{163,122,206},{169, 84,214},{136, 41,184},
            {140,  1,189},{135,  1,227},{ 93, 43,149},{ 49,  2,112},{156,158,197},{ 72, 80,154},{ 47,  4,164},{ 89, 41,228},
            { 95,  1,197},{  1,  1,237},{ 54,  1,235},{ 44,  1,204},{  1,  1,199},{ 81,126,210},{  1, 64,168},{  1,100,168},
            { 39, 90,141},{  1, 45,157},{ 20, 20,125},{ 22, 35, 82},{125,125,209},{ 88, 88,182},{ 87, 87,221},{133,168,181},
            { 82,129,130},{  1, 92, 92},{112,190,192},{ 73,192,154},{  1,192,193},{  1,161,202},{  1,131,131},{119,160,160},
            {112,191,116},{  1,193,155},{ 33,163,124},{ 33,163, 80},{ 36,130, 86},{ 83,129, 83},{121,162,121},{ 72,156, 16},
            {  1,193,  1},{ 34,163, 34},{  1,132, 38},{151,190, 74},{165,190,  1},{120,162, 34},{120,162, 79},{113,149,  1},
            { 70,113,  1},{ 90, 90, 40},{ 59, 59, 44},{ 43, 43,  1},{ 33, 56, 26},{  1, 45, 45},{ 39, 91, 91},{188,188,151},
            {188,188,113},{189,189, 73},{185,175, 76},{189,189,  1},{161,161, 34},{128,128, 37},{144,118, 44},{194,158,  1},
            {201,122, 80},{202,122, 35},{196,109,  1},{199, 98, 30},{210, 83, 36},{210, 83,  1},{183,183,165},{176,176,162},
            {166,144,112},{193,157,119},{174, 86, 38},{165, 54,  5},{151,148, 85},{135, 89, 40},{ 98, 44, 44},{ 98,  4,  1},
            {193,156,156},{202,122,122},{209, 83, 83},{200,122,164},{199,121,202},{208, 82,128},{212,  1,212},{214, 37,132},
            {216,  1, 86},{174, 38,220},{  7,186,185},{ 98,193,  1},{185,189,  1},{205,103,  1},{207, 61,207},{ 86,  4,224},
        };
        // various textures disabled because of the apparent non-linear interpolation of colors ingame, makes the image turn out way worse with non tri-symmetrical colors
        public struct obbase_texture{
            public obbase_texture(string _name, string _color, byte _r, byte _g, byte _b){
                name = _name;color = _color;r = _r; g = _g; b = _b;}
            public string name;
            public string color;
            public byte r; public byte g; public byte b;
        }
        public static obbase_texture[] observed_textures = new obbase_texture[] {
            //new obbase_texture("Brushed Painted Metal",  "red",          97, 35, 35),
            //new obbase_texture("Rust 01",                "brown",       125, 73, 43),
            //new obbase_texture("Copper Metal Scratched", "orange",      161, 89,  4),
            //new obbase_texture("UNSC Pain Thick Yellow", "yellow",      174,119, 29),
            //new obbase_texture("Enamel Smooth",          "green",       102,186, 22),
            //new obbase_texture("Plastic",                "pickle",       85,102, 26),
            new obbase_texture("Metal Smooth",           "metal?",       80, 84, 75),
            new obbase_texture("Metal Rough",            "cement",       93, 92, 73),
            //new obbase_texture("Orange Peel Finish",     "purple",      147, 87,202),
            new obbase_texture("Plastic Techsuit",       "black",        29, 29, 29),
            new obbase_texture("Gun Metal Painted",      "dark gray",    47, 47, 57),
            new obbase_texture("Paint",                  "gray",        110,110,110),
            new obbase_texture("Brushed",                "light gray",  150,150,150),
            new obbase_texture("Snowman Snow",           "kinda white", 170,170,170),
        };
        public struct base_texture{
            public base_texture(string _name, string _color, float _r, float _g, float _b){
                name = _name; color = _color; r = _r; g = _g; b = _b;}
            public string name;
            public string color;
            public float r; public float g; public float b;
        }
        public static base_texture[] textures = new base_texture[] {
            //new base_texture("Enamel Smooth",    "green",       0.48627450f, 0.96078431f, 0.03921568f),
            new base_texture("Plastic Techsuit", "black",       0.07176145f, 0.07176145f, 0.07176145f),
            new base_texture("Paint",            "gray",        0.2195197f,  0.2195197f,  0.21951972f),
            new base_texture("Brushed",          "light gray",  0.6120656f,  0.6120656f,  0.6120656f),
            new base_texture("Snowman Snow",     "kinda white", 0.7969165f,  0.7969165f,  0.79691654f),
        };




        // forge_palette_index[128], color_intensity[100], RGB[3]
        float[,,] intensity_color_list = new float[128,101,3];


        public struct mapped_object{
            public int color_index;
            public int intensity_index;
            public double X;
            public double Y;
        }
        
        public class return_object{
            public Bitmap? source_img;
            public Bitmap? visualized_img;

            public List<mapped_object>? pixels;
            public string? output_message;
            public int pixel_count;
            public int visible_pixel_count;
            public double image_accuracy = 0.0f;
            

        }

        public return_object result = new ();


        public return_object pixel_queue(string file_directory, float image_intensity, bool use_observables, int selected_color_index, float intensity_penalty){

            result.source_img = new Bitmap(file_directory);
            result.visualized_img = new Bitmap(file_directory);

            // build table based on whether we want theoretical colors, or colors observed ingame
            if (use_observables) build_observable_intensity_table(observed_textures[selected_color_index].r / 255f, observed_textures[selected_color_index].g / 255f, observed_textures[selected_color_index].b / 255f);
            else build_intensity_table(textures[selected_color_index].r, textures[selected_color_index].g, textures[selected_color_index].b);

            result.pixel_count = result.source_img.Width * result.source_img.Height;
            result.pixels = new List<mapped_object>();
            result.visible_pixel_count = result.pixel_count;

            
            if (result.pixel_count > 10000){
                result.output_message = "more than 10000 pixels, you should NOT attempt to process images with more than 10k pixels";
                result.pixels = null;
                return result;
            }

            int curr_pixel = 0;
            for (int x=0; x < result.source_img.Width; x++){
                for (int y = 0; y < result.source_img.Height; y++){
                    // check if pixel is transparent, if so skip it
                    Color pixel = result.source_img.GetPixel(x, y);
                    if (pixel.A == 0) {
                        curr_pixel++;
                        result.visible_pixel_count--;
                        result.image_accuracy += 1.0 / result.pixel_count;
                        result.visualized_img.SetPixel(x, y, Color.FromArgb(0x00, 0xff, 0x00));
                        continue;
                    }

                    //int color_index = get_index_of_closest_color(myBitmap.GetPixel(x, y));
                    //visualizedBitmap.SetPixel(x, y, color_by_list_index(color_index));
                    KeyValuePair<int, int> color_index = get_index_and_intensity_of_closest_color(pixel, image_intensity, intensity_penalty);
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

        #region PALETTE_INTENSITY_COLORS
        void build_intensity_table(float base_r, float base_g, float base_b){
            for (int i = 0; i < color_list.Length / 3; i++){ // my c sharp brothers in christ, why does length tell me the length OF THE WHOLE THING
                float r = color_list[i, 0];
                float g = color_list[i, 1];
                float b = color_list[i, 2];

                for (int intensity = 0; intensity <= 100; intensity++){
                    float intensity_f = (float)intensity / 100;


                    // interpolate between base color and new
                    // 

                    intensity_color_list[i,intensity,0] = interpolate_colors(base_r, r, intensity_f);
                    intensity_color_list[i,intensity,1] = interpolate_colors(base_g, g, intensity_f);
                    intensity_color_list[i,intensity,2] = interpolate_colors(base_b, b, intensity_f);
                }
        }}
        void build_observable_intensity_table(float base_r, float base_g, float base_b){
            for (int i = 0; i < observed_color_list.Length / 3; i++){
                float r = (float)observed_color_list[i, 0] / 255f;
                float g = (float)observed_color_list[i, 1] / 255f;
                float b = (float)observed_color_list[i, 2] / 255f;

                for (int intensity = 0; intensity <= 100; intensity++){
                    float intensity_f = (float)intensity / 100;


                    // interpolate between base color and new
                    // 

                    intensity_color_list[i,intensity,0] = interpolate_colors(base_r, r, intensity_f);
                    intensity_color_list[i,intensity,1] = interpolate_colors(base_g, g, intensity_f);
                    intensity_color_list[i,intensity,2] = interpolate_colors(base_b, b, intensity_f);
                }
        }}

        
        float interpolate_colors(float A, float B, float factor){
            return A - ((A - B) * factor);
        }



        KeyValuePair<int, int> get_index_and_intensity_of_closest_color(Color og_color, float image_intensity, float penalty){
            float r = color_as_float(og_color.R) * image_intensity;
            float g = color_as_float(og_color.G) * image_intensity;
            float b = color_as_float(og_color.B) * image_intensity;

            int closest_palette_index = 0;
            int closest_intensity_index = 0;
            float? closest_match_distance = null;

            for (int i = 0; i < color_list.Length / 3; i++){ // my c sharp brothers in christ, why does length tell me the length OF THE WHOLE THING

                for (int intensity = 0; intensity <= 100; intensity++){
                    float i_r = intensity_color_list[i, intensity, 0];
                    float i_g = intensity_color_list[i, intensity, 1];
                    float i_b = intensity_color_list[i, intensity, 2];

                    float distance = float_rb_dist(r, i_r) + float_rb_dist(g, i_g) + float_rb_dist(b, i_b);
                    distance += ((100.0f - intensity)/100) * penalty;
                    distance = Math.Clamp(distance, 0f, 1f);

                    if (closest_match_distance == null || distance < closest_match_distance){
                        closest_palette_index = i;
                        closest_intensity_index = intensity;
                        closest_match_distance = distance;
                    }
                }
            }
            float r_i_r = intensity_color_list[closest_palette_index, closest_intensity_index, 0];
            float r_i_g = intensity_color_list[closest_palette_index, closest_intensity_index, 1];
            float r_i_b = intensity_color_list[closest_palette_index, closest_intensity_index, 2];
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
