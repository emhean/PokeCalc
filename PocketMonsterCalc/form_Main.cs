using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PokeCalc
{
    public partial class form_Main : Form
    {
        StatsCalculator calc = new StatsCalculator();
        Pokemon pokemon_current;
        List<Pokemon> pokemon_list;
        string tbx_previous_value; // Used when user enters a non-numerical value on IV/EV textboxes.

        public form_Main()
        {
            InitializeComponent();
            UI_Init();
        }

        #region User Interface code

        /// <summary>
        /// Initialize the user interface.
        /// </summary>
        void UI_Init()
        {
            for (int i = 1; i < 101; ++i)
            {
                cbx_Level.Items.Add(i);
            }

            pokemon_list = new List<Pokemon>();
            XmlDocument doc = new XmlDocument();
            doc.Load("data/pokemon.xml");

            var pkmns = doc.GetElementsByTagName("pkmn");
            foreach(XmlElement p in pkmns)
            {
                var xp = new Pokemon(
                    int.Parse(p.GetAttribute("dexid")),
                    p.GetAttribute("name"),
                    p.GetAttribute("type1"),
                    p.GetAttribute("type2"),
                    new Stats(
                        int.Parse(p.GetAttribute("hp")),
                        int.Parse(p.GetAttribute("atk")),
                        int.Parse(p.GetAttribute("def")),
                        int.Parse(p.GetAttribute("spatk")),
                        int.Parse(p.GetAttribute("spdef")),
                        int.Parse(p.GetAttribute("spd"))
                        ));

                pokemon_list.Add(xp);
            }

            // Add to combo list
            for(int i = 0; i < pokemon_list.Count; ++i)
            {
                cbx_Pokemon.Items.Add(string.Format("[{0}] {1}", pokemon_list[i].dexID, pokemon_list[i].name));
            }


            #region Assign IV/EV editable textbox behaviour
            foreach (Control gbox in Controls)
            {
                if(gbox is GroupBox groupBox)
                {
                    foreach (var control in gbox.Controls)
                    {
                        if (control is TextBox textBox)
                        {
                            if (textBox.Name.StartsWith("tbx_IV")
                                || textBox.Name.StartsWith("tbx_EV"))
                            {
                                textBox.Click += UI_FocusTextBox;
                                textBox.KeyPress += UI_CheckIfEnterKey;
                            }
                        }
                    }
                }
            }
            #endregion

        }


        void UI_UpdateIV()
        {
            pokemon_current.stats_iv = StatTextboxParser(
                    tbx_IV_HP,
                    tbx_IV_Atk,
                    tbx_IV_Def,
                    tbx_IV_SpAtk,
                    tbx_IV_SpDef,
                    tbx_IV_Speed);

            CalculateLevels(pokemon_current);
            UI_SetFinalStats(UI_GetSelectedLevel());
        }

        void UI_UpdateEV()
        {
            pokemon_current.stats_ev = StatTextboxParser(
                tbx_EV_HP,
                tbx_EV_Atk,
                tbx_EV_Def,
                tbx_EV_SpAtk,
                tbx_EV_SpDef,
                tbx_EV_Speed);

            CalculateLevels(pokemon_current);
            UI_SetFinalStats(UI_GetSelectedLevel());
        }

        #region Getters
        /// <summary>
        /// Gets the selected level from level combobox.
        /// </summary>
        /// <returns>The level in a non-index value.</returns>
        int UI_GetSelectedLevel()
        {
            return cbx_Level.SelectedIndex + 1;
        }
        /// <summary>
        /// [Rarely used] Returns the selected index from level combobox.
        /// </summary>
        int UI_GetSelectedLevelIndex()
        {
            return cbx_Level.SelectedIndex;
        }

        /// <summary>
        /// Get the selected index from pokemon combobox.
        /// <para>Since this list is dependent on top-down loading there is no real value alternative method.</para>
        /// </summary>
        int UI_GetSelectedPokemonIndex()
        {
            return cbx_Pokemon.SelectedIndex;
        }
        #endregion


        #region Setters
        /// <summary>
        /// Set the image of the pokemon.
        /// </summary>
        void UI_SetImage(string pokemonName)
        {
            pic_Pokemon.Image = Image.FromFile(string.Format("data/gif/{0}.gif", pokemonName));
        }

        /// <summary>
        /// Set the pokemon-types images for the pictureboxes
        /// </summary>
        void UI_SetTypes(string type1, string type2)
        {
            if (!string.IsNullOrEmpty(type1))
                pic_Type1.Image = GetTypeBitmap(type1);

            if (!string.IsNullOrEmpty(type2))
                pic_Type2.Image = GetTypeBitmap(type2);
        }

        /// <summary>
        /// Sets the textboxes text from a stats struct.
        /// </summary>
        void UI_SetTextboxesFromStat(Stats stats, TextBox hp, TextBox atk, TextBox def, TextBox spAtk, TextBox spDef, TextBox spd)
        {
            hp.Text = stats.HP.ToString();
            atk.Text = stats.Attack.ToString();
            def.Text = stats.Defence.ToString();
            spAtk.Text = stats.SpAtk.ToString();
            spDef.Text = stats.SpDef.ToString();
            spd.Text = stats.Speed.ToString();
        }


        // Design choice:
        // Since some code actually GETS the value from the pokemon stats struct  
        //  - and SETS the value to the textboxes....?
        // Conclusion:
        // These methods is a Setter for the UI and will therefore be in the setter region.

        /// <summary>
        /// Sets the final stats textboxes based of a level.
        /// </summary>
        void UI_SetFinalStats(int level)
        {
            tbx_Stat_HP.Text = calc[level].HP.ToString();
            tbx_Stat_Atk.Text = calc[level].Attack.ToString();
            tbx_Stat_Def.Text = calc[level].Defence.ToString();
            tbx_Stat_SpAtk.Text = calc[level].SpAtk.ToString();
            tbx_Stat_SpDef.Text = calc[level].SpDef.ToString();
            tbx_Stat_Speed.Text = calc[level].Speed.ToString();
        }

        /// <summary>
        /// Sets the base-stats textboxes from pokemon.
        /// </summary>
        void UI_SetBaseStats()
        {
            tbx_BaseHP.Text = pokemon_current.stats_base.HP.ToString();
            tbx_BaseAtk.Text = pokemon_current.stats_base.Attack.ToString();
            tbx_BaseDef.Text = pokemon_current.stats_base.Defence.ToString();
            tbx_BaseSpAtk.Text = pokemon_current.stats_base.SpAtk.ToString();
            tbx_BaseSpDef.Text = pokemon_current.stats_base.SpDef.ToString();
            tbx_BaseSpeed.Text = pokemon_current.stats_base.Speed.ToString();
        }

        /// <summary>
        /// Sets the current pokemon based of index
        /// </summary>
        /// <param name="index">Use Get</param>
        void UI_SetPokemon(int index)
        {
            pokemon_current = pokemon_list[index]; // Copy from stack

            CalculateLevels(pokemon_current);

            UI_SetFinalStats(1);
            UI_SetBaseStats();

            UI_SetTextboxesFromStat(pokemon_current.stats_ev,
                tbx_EV_HP,
                tbx_EV_Atk,
                tbx_EV_Def,
                tbx_EV_SpAtk,
                tbx_EV_SpDef,
                tbx_EV_Speed);

            UI_SetTextboxesFromStat(pokemon_current.stats_iv,
                tbx_IV_HP,
                tbx_IV_Atk,
                tbx_IV_Def,
                tbx_IV_SpAtk,
                tbx_IV_SpDef,
                tbx_IV_Speed);

            UI_SetImage(pokemon_current.name.ToLower());

            cbx_Level.SelectedIndex = 0;

            UI_SetTypes(pokemon_current.type1, pokemon_current.type2);
        }
        #endregion


        #region Event Hooks
        /// <summary>
        /// Focus the textbox to make it editable.
        /// </summary>
        void UI_FocusTextBox(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.ReadOnly = false;
                textBox.Focus();
                tbx_previous_value = textBox.Text;
                textBox.SelectAll();
            }
        }

        void UI_CheckIfEnterKey(object sender, KeyPressEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (e.KeyChar == (char)Keys.Enter && textBox.ReadOnly)
                {
                    UI_FocusTextBox(textBox, EventArgs.Empty);
                }
                else if (e.KeyChar == (char)Keys.Enter)
                {
                    // Check if valid Value
                    if (int.TryParse(textBox.Text, out int r))
                    {
                        // We use the name of the control to determine whether its a textbox
                        // for the EV values or IV values
                        if (textBox.Name.StartsWith("tbx_EV"))
                        {
                            gbox_EV.Focus();

                            if (r < 0)
                            {
                                r = 0;
                                textBox.Text = "0";
                            }
                            if (r > 252)
                            {
                                r = 252;
                                textBox.Text = "252";
                            }
                        }
                        else if (textBox.Name.StartsWith("tbx_IV"))
                        {
                            gbox_IV.Focus();

                            if (r < 0)
                            {
                                r = 0;
                                textBox.Text = "0";
                            }
                            if (r > 31)
                            {
                                r = 31;
                                textBox.Text = "31";
                            }
                        }

                        UI_UpdateEV();
                        UI_UpdateIV();
                    }
                    else
                    {
                        textBox.Text = tbx_previous_value;
                    }

                    textBox.ReadOnly = true;
                }
            }

        }


        #endregion


        #endregion // End of region User Interface code




        Bitmap GetTypeBitmap(string type)
        {
            switch(type)
            {
                case "Bug": return Properties.Resources.type_bug;
                case "Dark": return Properties.Resources.type_dark;
                case "Dragon": return Properties.Resources.type_dragon;
                case "Electric": return Properties.Resources.type_electric;
                case "Fairy": return Properties.Resources.type_fairy;
                case "Fighting": return Properties.Resources.type_fighting;
                case "Fire": return Properties.Resources.type_fire;
                case "Flying": return Properties.Resources.type_flying;
                case "Ghost": return Properties.Resources.type_ghost;
                case "Grass": return Properties.Resources.type_grass;
                case "Ground": return Properties.Resources.type_ground;
                case "Ice": return Properties.Resources.type_ice;
                case "Normal": return Properties.Resources.type_normal;
                case "Poison": return Properties.Resources.type_poison;
                case "Psychic": return Properties.Resources.type_psychic;
                case "Rock": return Properties.Resources.type_rock;
                case "Steel": return Properties.Resources.type_steel;
                case "None": return Properties.Resources.type_none;
                case "Water": return Properties.Resources.type_water;
            }

            throw new Exception("Type does not exist!");
        }

        Stats StatTextboxParser(TextBox hp, TextBox atk, TextBox def, TextBox spAtk, TextBox spDef, TextBox spd)
        {
            return new Stats(
                int.Parse(hp.Text),
                int.Parse(atk.Text),
                int.Parse(def.Text),
                int.Parse(spAtk.Text),
                int.Parse(spDef.Text),
                int.Parse(spd.Text));
        }

        void CalculateLevels(Pokemon pokemon)
        {
            calc.Update(pokemon); // Update calculator
        }

        private void cbx_Level_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateLevels(pokemon_current);
            UI_SetFinalStats(cbx_Level.SelectedIndex + 1);
        }

        private void cbx_Pokemon_SelectedIndexChanged(object sender, EventArgs e)
        {
            UI_SetPokemon( UI_GetSelectedPokemonIndex());
        }



        private void btn_IV_EV_Update_Click(object sender, EventArgs e)
        {
            UI_UpdateIV();
            UI_UpdateEV();
        }

        private void sSDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines("data/pokemon_names.txt");
            string url = "https://www.smogon.com/dex/media/sprites/xy/";

            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].StartsWith("#"))
                    continue;

                try
                {
                    System.Net.WebRequest request = System.Net.WebRequest.Create(url + string.Format("{0}.gif", lines[i].ToLower()));
                    System.Net.WebResponse response = request.GetResponse();
                    System.IO.Stream responseStream = response.GetResponseStream();

                    Image image = Image.FromStream(responseStream);
                    Save(image, string.Format("data/gif/{0}.gif", lines[i].ToLower()));
                }
                catch
                {
                }
            }


            return;



            ////var web = new WebClient();
            ////string[] headers = File.ReadAllLines("headers.txt");
            ////WebHeaderCollection coll = new WebHeaderCollection();
            ////coll.Set(HttpRequestHeader.Accept, lines[0]);
            ////coll.Set(HttpRequestHeader.AcceptEncoding, lines[1]);
            ////coll.Set(HttpRequestHeader.AcceptLanguage, lines[2]);
            ////coll.Set(HttpRequestHeader.CacheControl, lines[3]);
            ////coll.Set(HttpRequestHeader.Connection, lines[4]);
            ////coll.Set(HttpRequestHeader.Date, string.Empty);
            ////coll.Set(HttpRequestHeader.Host, lines[6]);
            ////coll.Set(HttpRequestHeader.IfModifiedSince, string.Empty);
            ////coll.Set(HttpRequestHeader.IfNoneMatch, lines[8]);
            ////coll.Set(HttpRequestHeader.Upgrade, lines[9]);
            ////coll.Set(HttpRequestHeader.UserAgent, lines[10]);
            ////web.Headers = coll;


            ////
            ////for(int i = 0; i < headers.Length; ++i)
            ////{
            ////    web.Headers.Add(headers[i]);
            ////}

            //string url;
            //string file;



            //for (int i = 0; i < lines.Length; ++i)
            //{
            //    if (lines[i].StartsWith("#"))
            //        continue;

            //    url = "https://www.smogon.com/dex/media/sprites/xy/";
            //    file = string.Format("{0}.gif", lines[i].ToLower());



            //    //// Open a Stream and decode a GIF image
            //    //Stream imageStreamSource = new FileStream("tulipfarm.gif", FileMode.Open, FileAccess.Read, FileShare.Read);
            //    //GifBitmapDecoder decoder = new GifBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            //    //BitmapSource bitmapSource = decoder.Frames[0];

            //    //// Draw the Image
            //    //Image myImage = new Image();
            //    //myImage.Source = bitmapSource;
            //    //myImage.Stretch = Stretch.None;
            //    //myImage.Margin = new Thickness(20);

            //    try
            //    {
            //        System.Net.WebRequest request = System.Net.WebRequest.Create(url + file);
            //        System.Net.WebResponse response = request.GetResponse();
            //        System.IO.Stream responseStream = response.GetResponseStream();

            //        Bitmap bitmap2 = new Bitmap(responseStream);
            //        bitmap2.Save(string.Format("data/gif/{0}.gif", lines[i].ToLower()));
            //    }
            //    catch
            //    {
            //        continue;
            //    }

            //}

            
        }

        private void cbox_AutoUpdateValues_CheckedChanged(object sender, EventArgs e)
        {
            btn_IV_EV_Update.Visible = !(sender as CheckBox).Checked;
        }



        void Save(Image image, string fileName)
        {
            // Gdi+ constants absent from System.Drawing.
            const int PropertyTagFrameDelay = 0x5100;
            const int PropertyTagLoopCount = 0x5101;
            const short PropertyTagTypeLong = 4;
            const short PropertyTagTypeShort = 3;

            const int UintBytes = 4;

            //...

            List<Bitmap> Bitmaps = new List<Bitmap>();

            int frameCount = image.GetFrameCount(System.Drawing.Imaging.FrameDimension.Time);
            for (int i = 0; i < frameCount; ++i)
            {
                image.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Time, i);
                var b = new Bitmap(image);
                Bitmaps.Add(b);
            }


            var gifEncoder = GetEncoder(ImageFormat.Gif);
            // Params of the first frame.
            var encoderParams1 = new EncoderParameters(1);
            encoderParams1.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
            // Params of other frames.
            var encoderParamsN = new EncoderParameters(1);
            encoderParamsN.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionTime);
            // Params for the finalizing call.
            var encoderParamsFlush = new EncoderParameters(1);
            encoderParamsFlush.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.Flush);

            // PropertyItem for the frame delay (apparently, no other way to create a fresh instance).
            var frameDelay = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
            frameDelay.Id = PropertyTagFrameDelay;
            frameDelay.Type = PropertyTagTypeLong;
            // Length of the value in bytes.
            frameDelay.Len = Bitmaps.Count * UintBytes;
            // The value is an array of 4-byte entries: one per frame.
            // Every entry is the frame delay in 1/100-s of a second, in little endian.
            frameDelay.Value = new byte[Bitmaps.Count * UintBytes];
            // E.g., here, we're setting the delay of every frame to 1 second.
            var frameDelayBytes = BitConverter.GetBytes((uint)0);
            for (int j = 0; j < Bitmaps.Count; ++j)
                Array.Copy(frameDelayBytes, 0, frameDelay.Value, j * UintBytes, UintBytes);

            // PropertyItem for the number of animation loops.
            var loopPropertyItem = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
            loopPropertyItem.Id = PropertyTagLoopCount;
            loopPropertyItem.Type = PropertyTagTypeShort;
            loopPropertyItem.Len = 1;
            // 0 means to animate forever.
            loopPropertyItem.Value = BitConverter.GetBytes((ushort)0);

            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                bool first = true;
                Bitmap firstBitmap = null;
                // Bitmaps is a collection of Bitmap instances that'll become gif frames.
                foreach (var bitmap in Bitmaps)
                {
                    if (first)
                    {
                        firstBitmap = bitmap;
                        firstBitmap.SetPropertyItem(frameDelay);
                        firstBitmap.SetPropertyItem(loopPropertyItem);
                        firstBitmap.Save(stream, gifEncoder, encoderParams1);
                        first = false;
                    }
                    else
                    {
                        firstBitmap.SaveAdd(bitmap, encoderParamsN);
                    }
                }
                firstBitmap.SaveAdd(encoderParamsFlush);
            }
        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

    }
}
