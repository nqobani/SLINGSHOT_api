﻿using SlingshotAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace SlingshotAPI.ApplicationLogicLayer.Services
{
    public class VCard
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public byte[] Image { get; set; }
        public string ImageLink { get; set; }
    }

    public class VcardManager
    {
        public static void GenearateVCard(VCard vCard)
        {
           // var t = HostingEnvironment.MapPath("~\\Services\\vCard\\vCard.vcf");
            using (var vCardFile = File.OpenWrite(@"C:\Users\User\Music\images\vCard.vcf"))
            using (var swWriter = new StreamWriter(vCardFile))
            {
                swWriter.Write(BuildVCard(vCard));
            }
        }
        public static void LoadVCardData(VCardModel vCardM)
        {
            var vcMyCard = new VCard
            {
                FName = vCardM.firstName,
                LName = vCardM.lastName,
                Company = vCardM.company,
                JobTitle = vCardM.jobTitle,
                Address = vCardM.webPageAddress,
                City = vCardM.city,
                Country = vCardM.country,
                Phone = vCardM.businessPhoneNumber,
                Mobile = vCardM.mobilePhoneNumber,
                Email = vCardM.email,
                ImageLink = vCardM.profilePicturePath
            };

            GenearateVCard(vcMyCard);
        }
        public static string BuildVCard(VCard vCard)
        {
            vCard.Image = File.ReadAllBytes(vCard.ImageLink);
            var vCardBuilder = new StringBuilder();
            vCardBuilder.AppendLine("BEGIN:VCARD");
            vCardBuilder.AppendLine("VERSION:2.1");
            vCardBuilder.AppendLine("N:" + vCard.LName + ";" + vCard.FName);
            vCardBuilder.AppendLine("FN:" + vCard.FName + " " + vCard.LName);
            vCardBuilder.Append("ADR;HOME;PREF:;;");
            vCardBuilder.Append(vCard.Address + ";");
            vCardBuilder.Append(vCard.City + ";;");
            vCardBuilder.AppendLine(vCard.Country);
            vCardBuilder.AppendLine("ORG:" + vCard.Company);
            vCardBuilder.AppendLine("TITLE:" + vCard.JobTitle);
            vCardBuilder.AppendLine("TEL;HOME;VOICE:" + vCard.Phone);
            vCardBuilder.AppendLine("TEL;CELL;VOICE:" + vCard.Mobile);
            vCardBuilder.AppendLine("EMAIL;PREF;INTERNET:" + vCard.Email);

            vCardBuilder.AppendLine("PHOTO;ENCODING=BASE64;TYPE=JPEG:");
            vCardBuilder.AppendLine(Convert.ToBase64String(vCard.Image));
            vCardBuilder.AppendLine(string.Empty);
            vCardBuilder.AppendLine(string.Empty);
            vCardBuilder.AppendLine(string.Empty);
            vCardBuilder.AppendLine("END:VCARD");
            return vCardBuilder.ToString();
        }
    }
}
