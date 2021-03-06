﻿using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MahTweets.Core;
using MahTweets.Core.Composition;
using MahTweets.Core.Interfaces.Settings;

namespace MahTweets.TweetProcessors.AdditionalSmarts
{
    public class TitleFromWikipedia : IAdditonalTextSmarts
    {
        private readonly IAdditonalSmartsSettingsProvider _additonalSmartsSettingsProvider;

        public TitleFromWikipedia() 
        {
            _additonalSmartsSettingsProvider = CompositionManager.Get<IAdditonalSmartsSettingsProvider>(); //get & cache    
        }

        #region IAdditonalTextSmarts Members

        public string Name
        {
            get { return "wikipedia"; }
        }

        public int Priority
        {
            get { return 2; }
        }

        public bool CanPageTitle(string url)
        {
            return url != null && _additonalSmartsSettingsProvider.AdditionalSmartsMapping.AsParallel().Any(u => (url.ToLower().Contains(u.Url.ToLower()) && u.ProcessType == Name));
        }

        public async Task<string> Process(String url)
        {
            var fetcher = new AsyncWebFetcher();
            var page = await fetcher.FetchAsync(url);
            if (page == null) return null;

            var titleRegex = new Regex(@"<h1 id=\x22firstHeading\x22.*>(?<title>.*)<\/h1>", RegexOptions.IgnoreCase);
            var match = titleRegex.Match(page);

            if (!match.Success) return null;
            var title = WebUtility.HtmlDecode(match.Groups["title"].Value);

            return title;
        }

        #endregion
    }
}