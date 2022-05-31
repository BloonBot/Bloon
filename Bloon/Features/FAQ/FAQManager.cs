namespace Bloon.Features.FAQ
{
    using System.Collections.Generic;

    public class FAQManager
    {
        private readonly Dictionary<int, Faq> faqRegexes = new Dictionary<int, Faq>();

        public void Init(List<Faq> faqs)
        {
            faqs.ForEach((Faq faq) => this.AddFaq(faq));
        }

        public void Reset()
        {
            this.faqRegexes.Clear();
        }

        public void AddFaq(Faq faq)
        {
            this.faqRegexes.Add(faq.Id, faq);
        }

        public bool TryForAutoResponse(string message, out string response)
        {
            response = string.Empty;

            foreach (Faq faq in this.faqRegexes.Values)
            {
                if (faq.Regex.IsMatch(message))
                {
                    response = faq.Message;
                    return true;
                }
            }

            return false;
        }
    }
}
