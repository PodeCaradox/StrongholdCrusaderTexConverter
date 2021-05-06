using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using TexEditor.Model;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TexEditor
{
    public static class LibTex
    {
        public static TexSection[] Deserialize(string filePath, int maxSections = 259)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            var arrayDummy = File.ReadAllBytes(filePath);

            //Sections
            var offsets = new uint[maxSections+1];
            //Sections
            int offset = 0;
            // read offsets
            //offsets are from/to
            //0-36 36 - ...
            for (int i = 0; i < maxSections+1; i++)
            {
                offsets[i] = BitConverter.ToUInt32(arrayDummy, offset);
                offset += 4;
            }

            var result = new TexSection[maxSections];

            // read strings (TODO, a bit bodged)
            for (int i = 0; i < maxSections; i++)
            {
                uint min, max;
                min = offsets[i];
                max = offsets[i + 1];
            
                result[i] = new TexSection();

                var sectionText = new List<string>();
                result[i].SectionString = sectionText;
                result[i].SectionIndex = i;

                // skip offsets.Length * sizeof(uint)
                //reader.BaseStream.Seek(MaxSectionIndex * sizeof(uint) + (offsets[i] * 2), SeekOrigin.Begin);
                var line = new StringBuilder();
                for (uint rp = min; rp < max; rp++)
                {
                    var code = new byte[] { arrayDummy[offset], arrayDummy[offset+1] };
                    offset += 2;
                    if (code.Length == 0)// End of the file
                    {
                        sectionText.Add(null);
                        break;
                    }

                    if (code[0] == 0 && code[1] == 0)// End of the string
                    {
                        if (line.Length == 0)
                        {
                            continue;
                        }
                        sectionText.Add(line.ToString());
                        line.Clear();
                    }
                    else
                    {
                        line.Append(Encoding.Unicode.GetString(code));
                    }
                }
            }

            return result;
        }

        public static void Serialize(string filePath, int maxSections= 259)
        {
            String file = File.ReadAllText(filePath.Replace(".tex", ".txt"));

     
            var texSection = JsonConvert.DeserializeObject<TexSection[]>(file);
            //Sections
            var offsets = new uint[maxSections+1];
            offsets[0] = 0;
            List<byte> data = new List<byte>();
            uint calculateOffset = 0;
            for (int i = 0; i < maxSections; i++)
            {
                
                for (int j = 0; j < texSection[i].SectionString.Count; j++)
                {
                    for (int z = 0; z < texSection[i].SectionString[j].Length; z++)
                    {
                        data.AddRange(BitConverter.GetBytes(texSection[i].SectionString[j][z]));
                        calculateOffset++;
                    }
                    data.AddRange(new byte[] { 0, 0 });
                    calculateOffset++;
                }
                offsets[i + 1] = offsets[i] + calculateOffset;
                calculateOffset = 0;

            }
            List<byte> offsetsinByte = new List<byte>();
            for (int i = 0; i < offsets.Length; i++)
            {
                offsetsinByte.AddRange(BitConverter.GetBytes(offsets[i]));
            }


            var allData = new List<byte>(offsetsinByte.Count +
                                         data.Count);
            allData.AddRange(offsetsinByte);
            allData.AddRange(data);

            File.WriteAllBytes(filePath, allData.ToArray());
        }

        public static string ConvertToText(this TexSection[] texSections)
        {
            string jsonString;
            jsonString = JsonConvert.SerializeObject(texSections,Formatting.Indented);
            return jsonString;
        }
    }
}
