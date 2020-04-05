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
        /// Initialize the user interface and imports pokemon from file.
        /// </summary>
        void UI_Init()
        {
            for (int i = 1; i < 101; ++i)
            {
                cbx_Level.Items.Add(i);
                cbx_IVFind_Level.Items.Add(i);
            }

            foreach(Nature nat in Nature.GetNatures())
            {
                cbx_Nature.Items.Add(nat);
                cbx_IVFind_Nature.Items.Add(nat);
            }

            cbx_Nature.SelectedIndex = 0;

            pokemon_list = UI_ImportPokemonFromTxt("data/stats.txt");
            UI_AddPokemonToList(pokemon_list);


            #region Assign IV/EV editable textbox behaviour
            tbx_EV_HP.Click += UI_FocusTextBox;
            tbx_EV_HP.KeyPress += UI_CheckIfEnterKey;
            tbx_EV_Atk.Click += UI_FocusTextBox;
            tbx_EV_Atk.KeyPress += UI_CheckIfEnterKey;
            tbx_EV_Def.Click += UI_FocusTextBox;
            tbx_EV_Def.KeyPress += UI_CheckIfEnterKey;
            tbx_EV_SpAtk.Click += UI_FocusTextBox;
            tbx_EV_SpAtk.KeyPress += UI_CheckIfEnterKey;
            tbx_EV_SpDef.Click += UI_FocusTextBox;
            tbx_EV_SpDef.KeyPress += UI_CheckIfEnterKey;
            tbx_EV_Speed.Click += UI_FocusTextBox;
            tbx_EV_Speed.KeyPress += UI_CheckIfEnterKey;

            tbx_IV_HP.Click += UI_FocusTextBox;
            tbx_IV_HP.KeyPress += UI_CheckIfEnterKey;
            tbx_IV_Atk.Click += UI_FocusTextBox;
            tbx_IV_Atk.KeyPress += UI_CheckIfEnterKey;
            tbx_IV_Def.Click += UI_FocusTextBox;
            tbx_IV_Def.KeyPress += UI_CheckIfEnterKey;
            tbx_IV_SpAtk.Click += UI_FocusTextBox;
            tbx_IV_SpAtk.KeyPress += UI_CheckIfEnterKey;
            tbx_IV_SpDef.Click += UI_FocusTextBox;
            tbx_IV_SpDef.KeyPress += UI_CheckIfEnterKey;
            tbx_IV_Speed.Click += UI_FocusTextBox;
            tbx_IV_Speed.KeyPress += UI_CheckIfEnterKey;
            #endregion
        }


        /// <summary>
        /// Create the shit file. Zygarde causes a crash fuck his 10% Forme and 50% Forme.
        /// </summary>
        /// <param name="file"></param>
        void UI_CreateFileFromShitFile(string file)
        {
            string[] lines = File.ReadAllLines(file);

            List<string> list_new = new List<string>();
            StringBuilder sb = new StringBuilder();
            string[] split;

            // For lazy way to check numbers later
            string[] numbers = new string[]
            {
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
            };

            for(int i = 0; i < lines.Length; ++i) // line index
            {
                sb.Clear(); // Clear the stringbuilder from previous iteration.

                split = lines[i].Split(' '); // line split

                sb.Append(split[0]); // append first element which is always dex id

                // This code is here to fix the problem with importing pokemon such as 
                // Charizard (Mega Charizard X)
                // Because there's an extra letter in the name which causes problem with the string split.
                // The code below finds the first digit after the name and sets the offset to match it.
                int offset = 0;

                List<int> exclusions = new List<int>();

                bool isValue = false;
                for (int n = 0; n < numbers.Length; ++n) 
                {
                    if (split[3].Contains(numbers[n]))
                        isValue = true;
                }

                if ( !isValue)
                {
                    exclusions.Add(3);

                    // Finds last index of name
                    for (int x = 4; x < split.Length; ++x)
                    {
                        exclusions.Add(x);
                        for (int n = 0; n < numbers.Length; ++n)  // iterate all digits. Might be a better way to do this.
                        {
                            if (split[x].StartsWith(numbers[n]))
                            {
                                offset = x - 3;
                                exclusions.Remove(x);
                                break;
                            }
                        }
                        if (offset != 0) // This to break the whole loop because value is set.
                            break;
                    }
                }

                for (int j = 2; j < 9 + offset; ++j) // split index
                {
                    bool append = true;

                    for(int ex = 0; ex < exclusions.Count; ++ex)
                    {
                        if(j == exclusions[ex])
                        {
                            append = false;
                            break;
                        }
                    }

                    if(append)
                    {
                        sb.Append(';'); // append space otherwise its all one single string.
                        sb.Append(split[j]);
                    }
                    else
                    {
                        sb.Append(' '); // append space otherwise its all one single string.
                        sb.Append(split[j]);
                    }
                }

                list_new.Add(sb.ToString()); // append the string from the stringbuilder.
            }


            File.WriteAllLines("data/stats.txt", list_new.ToArray());
        }


        /// <summary>
        /// DO NOT USE.
        /// </summary>
        List<Pokemon> UI_ImportPokemonFromXML(string file)
        {
            var list = new List<Pokemon>();

            // Old XML code
            //XmlDocument doc = new XmlDocument();
            //doc.Load("data/pokemon.xml");

            //var pkmns = doc.GetElementsByTagName("pkmn");
            //foreach(XmlElement p in pkmns)
            //{
            //    var xp = new Pokemon(
            //        int.Parse(p.GetAttribute("dexid")),
            //        p.GetAttribute("name"),
            //        p.GetAttribute("type1"),
            //        p.GetAttribute("type2"),
            //        new Stats(
            //            int.Parse(p.GetAttribute("hp")),
            //            int.Parse(p.GetAttribute("atk")),
            //            int.Parse(p.GetAttribute("def")),
            //            int.Parse(p.GetAttribute("spatk")),
            //            int.Parse(p.GetAttribute("spdef")),
            //            int.Parse(p.GetAttribute("spd"))
            //            ));

            //    list.Add(xp);
            //}

            // Add to combo list
            //for (int i = 0; i < pokemon_list.Count; ++i)
            //{
            //    cbx_Pokemon.Items.Add(string.Format("[{0}] {1}", list[i].dexID, list[i].name));
            //}

            return list;
        }


        List<Pokemon> UI_ImportPokemonFromTxt(string file)
        {
            List<Pokemon> list = new List<Pokemon>();

            string[] lines = File.ReadAllLines(file);

            for(int i = 0; i < lines.Length; ++i)
            {
                var p = new Pokemon(lines[i]);
                list.Add(p);
            }

            return list;
        }

        /// <summary>
        /// Adds pokemon to the combobox.
        /// </summary>
        void UI_AddPokemonToList(List<Pokemon> list)
        {
            for(int i = 0; i < list.Count; ++i)
            {
                //object item = string.Format("[{0}] {1}", list[i].dexID, list[i].name);
                //object item = string.Format("{0} [{1}]", list[i].name, list[i].dexID);
                var item = list[i];

                cbx_Pokemon.Items.Add(item);
            }
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

        Pokemon UI_GetSelectedPokemon()
        {
            return (Pokemon)cbx_Pokemon.Items[UI_GetSelectedPokemonIndex()];
        }

        Nature UI_GetSelectedNature()
        {
            return (Nature)cbx_Nature.Items[cbx_Nature.SelectedIndex];
        }
        #endregion


        #region Setters
        /// <summary>
        /// Set the image of the pokemon.
        /// </summary>
        void UI_SetImage(string pokemonName)
        {
            Image img;

            try
            {
                // This might throw an FileNotFound exception.
                img = Image.FromFile(string.Format("data/gif/{0}.gif", pokemonName));

                // If here, then file above was found.
                pic_Pokemon.Image = img;
            }
            catch
            {
                // Set image to MissingNO because no image for pokemon was found.
                img = PokeCalc.Properties.Resources.Missingno_RB;
            }

            pic_Pokemon.Image = img;
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
            var stats = calc.GetStats(level, pokemon_current);

            tbx_Stat_HP.Text = stats.HP.ToString();
            tbx_Stat_Atk.Text = stats.Attack.ToString();
            tbx_Stat_Def.Text = stats.Defence.ToString();
            tbx_Stat_SpAtk.Text = stats.SpAtk.ToString();
            tbx_Stat_SpDef.Text = stats.SpDef.ToString();
            tbx_Stat_Speed.Text = stats.Speed.ToString();
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
        void UI_SetPokemon()
        {
            pokemon_current = UI_GetSelectedPokemon(); //pokemon_list[index]; // Copy from stack

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
            calc.SetNature(UI_GetSelectedNature());
            calc.Update(pokemon); // Update calculator
        }

        private void cbx_Level_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateLevels(pokemon_current);
            UI_SetFinalStats(cbx_Level.SelectedIndex + 1);
        }
        private void cbx_Nature_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateLevels(pokemon_current);
            UI_SetFinalStats(cbx_Level.SelectedIndex + 1);


            var nat = UI_GetSelectedNature();
            UI_AdjustTextBoxToNature(nat.Attack, tbx_Stat_Atk);
            UI_AdjustTextBoxToNature(nat.Defence, tbx_Stat_Def);
            UI_AdjustTextBoxToNature(nat.SpAtk, tbx_Stat_SpAtk);
            UI_AdjustTextBoxToNature(nat.SpDef, tbx_Stat_SpDef);
            UI_AdjustTextBoxToNature(nat.Speed, tbx_Stat_Speed);
        }

        void UI_AdjustTextBoxToNature(float stat, TextBox textbox)
        {
            if (stat == 0.9f)
                textbox.BackColor = Color.IndianRed;
            else if (stat == 1.1f)
                textbox.BackColor = Color.LightGreen;
            else
                textbox.BackColor = Color.Silver;
        }


        private void cbx_Pokemon_SelectedIndexChanged(object sender, EventArgs e)
        {
            UI_SetPokemon();
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

        private void btn_IV_SetZero_Click(object sender, EventArgs e)
        {
            tbx_IV_HP.Text = "0";
            tbx_IV_Atk.Text = "0";
            tbx_IV_Def.Text = "0";
            tbx_IV_SpAtk.Text = "0";
            tbx_IV_SpDef.Text = "0";
            tbx_IV_Speed.Text = "0";
            UI_UpdateIV();
        }

        private void btn_IV_SetMax_Click(object sender, EventArgs e)
        {
            tbx_IV_HP.Text = "31";
            tbx_IV_Atk.Text = "31";
            tbx_IV_Def.Text = "31";
            tbx_IV_SpAtk.Text = "31";
            tbx_IV_SpDef.Text = "31";
            tbx_IV_Speed.Text = "31";
            UI_UpdateIV();
        }

        private void btn_EV_SetZero_Click(object sender, EventArgs e)
        {
            tbx_EV_HP.Text = "0";
            tbx_EV_Atk.Text = "0";
            tbx_EV_Def.Text = "0";
            tbx_EV_SpAtk.Text = "0";
            tbx_EV_SpDef.Text = "0";
            tbx_EV_Speed.Text = "0";
            UI_UpdateEV();
        }

        private void btn_EV_SetMax_Click(object sender, EventArgs e)
        {
            tbx_EV_HP.Text = "252";
            tbx_EV_Atk.Text = "252";
            tbx_EV_Def.Text = "252";
            tbx_EV_SpAtk.Text = "252";
            tbx_EV_SpDef.Text = "252";
            tbx_EV_Speed.Text = "252";
            UI_UpdateEV();
        }

        private void cbx_IVFind_Start_Click(object sender, EventArgs e)
        {
            int level = cbx_IVFind_Level.SelectedIndex + 1;
            Nature nat = (Nature)cbx_IVFind_Nature.Items[cbx_IVFind_Nature.SelectedIndex];
            var poke = UI_GetSelectedPokemon();

            var sc = new StatsCalculator();
            sc.SetNature(nat);

            FindIV(poke, level, sc, int.Parse(tbx_IVFind_HP.Text), tbx_IVFind_HPResult, Stats.STAT.HP);
            FindIV(poke, level, sc, int.Parse(tbx_IVFind_Atk.Text), tbx_IVFind_AtkResult, Stats.STAT.ATTACK);
            FindIV(poke, level, sc, int.Parse(tbx_IVFind_Def.Text), tbx_IVFind_DefResult, Stats.STAT.DEFENCE);
            FindIV(poke, level, sc, int.Parse(tbx_IVFind_SpAtk.Text), tbx_IVFind_SpAtkResult, Stats.STAT.SP_ATTACK);
            FindIV(poke, level, sc, int.Parse(tbx_IVFind_SpDef.Text), tbx_IVFind_SpDefResult, Stats.STAT.SP_DEFENCE);
            FindIV(poke, level, sc, int.Parse(tbx_IVFind_Spd.Text), tbx_IVFind_SpdResult, Stats.STAT.SPEED);
        }

        int FindIV(Pokemon poke, int level, StatsCalculator sc, int IV_ToFind, TextBox out_iv, Stats.STAT stat)
        {
            Stats old_iv = poke.stats_iv;
            poke.stats_iv = new Stats(0, 0, 0, 0, 0, 0);

            List<int> matches = new List<int>();

            for (int i = 0; i <= 32; ++i)
            {
                bool match;

                // If match
                if(stat == Stats.STAT.HP)
                {
                    poke.stats_iv.HP = i;
                    match = (sc.GetStats(level, poke).HP == IV_ToFind);
                }
                else if (stat == Stats.STAT.ATTACK)
                {
                    poke.stats_iv.Attack = i;
                    match = (sc.GetStats(level, poke).Attack == IV_ToFind);
                }
                else if (stat == Stats.STAT.DEFENCE)
                {
                    poke.stats_iv.Defence = i;
                    match = (sc.GetStats(level, poke).Defence == IV_ToFind);
                }
                else if (stat == Stats.STAT.SP_ATTACK)
                {
                    poke.stats_iv.SpAtk = i;
                    match = (sc.GetStats(level, poke).SpAtk == IV_ToFind);
                }
                else if (stat == Stats.STAT.SP_DEFENCE)
                {
                    poke.stats_iv.SpDef = i;
                    match = (sc.GetStats(level, poke).SpDef == IV_ToFind);
                }
                else
                {
                    poke.stats_iv.Speed = i;
                    match = (sc.GetStats(level, poke).Speed == IV_ToFind);
                }

                if(match)
                {
                    //out_iv.Text = i.ToString();
                    poke.stats_iv = old_iv; // Set to previous IV before calculations

                    matches.Add(i);
                    //return i; // Return value
                }
            }


            if (matches.Count > 1)
            {
                out_iv.Text = string.Format("{0}-{1}", matches[0], matches[matches.Count - 1]);
            }

            if (matches.Count != 0)
                return matches[0];
            else
                return 0;
        }
    }
}
