using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace IoC_asesment
{
    public class Speaker
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? Exp { get; set; }
        public bool HasBlog { get; set; }
        public string BlogURL { get; set; }
        public WebBrowser Browser { get; set; }
        public List<string> Certifications { get; set; }
        public string Employer { get; set; }
        public int RegistrationFee { get; set; }
        public List<Session> Sessions { get; set; }

        public int? Register(IRepository repository)
        {
            ValidateSpeaker();

            if (!IsSpeakerQualified())
                throw new SpeakerDoesntMeetRequirementsException("Speaker doesn't meet the requirements.");

            ApproveSessions();

            CalculateRegistrationFee();

            try
            {
                return repository.SaveSpeaker(this);
            }
            catch (Exception e)
            {
                // Log or handle the exception accordingly
                throw new SpeakerRegistrationFailedException("Failed to register speaker.", e);
            }
        }

        private void ValidateSpeaker()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                throw new ArgumentNullException(nameof(FirstName), "First Name is required.");
            if (string.IsNullOrWhiteSpace(LastName))
                throw new ArgumentNullException(nameof(LastName), "Last name is required.");
            if (string.IsNullOrWhiteSpace(Email))
                throw new ArgumentNullException(nameof(Email), "Email is required.");
        }

        private bool IsSpeakerQualified()
        {
            var requiredCertifications = 3;
            var validEmployers = new List<string>() { "Microsoft", "Google", "Fog Creek Software", "37Signals" };
            var invalidDomains = new List<string>() { "aol.com", "hotmail.com", "prodigy.com", "CompuServe.com" };

            return (Exp > 10 || HasBlog || Certifications.Count >= requiredCertifications || validEmployers.Contains(Employer))
                && (!invalidDomains.Contains(Email.Split('@').Last()) && !(Browser.Name == WebBrowser.BrowserName.InternetExplorer && Browser.MajorVersion < 9));
        }

        private void ApproveSessions()
        {
            if (Sessions == null || !Sessions.Any())
                throw new ArgumentException("Can't register speaker with no sessions to present.");

            var obsoleteTechnologies = new List<string>() { "Cobol", "Punch Cards", "Commodore", "VBScript" };

            foreach (var session in Sessions)
            {
                if (obsoleteTechnologies.Any(tech => session.Title.Contains(tech) || session.Description.Contains(tech)))
                {
                    session.Approved = false;
                }
                else
                {
                    session.Approved = true;
                }
            }
        }

        private void CalculateRegistrationFee()
        {

            RegistrationFee = Exp switch
            {
                <= 1 => 500,
                >= 2 and <= 3 => 250,
                >= 4 and <= 5 => 100,
                >= 6 and <= 9 => 50,
                _ => 0
            };
        }

        public class SpeakerDoesntMeetRequirementsException : Exception
        {
            public SpeakerDoesntMeetRequirementsException(string message) : base(message) { }
        }

        public class SpeakerRegistrationFailedException : Exception
        {
            public SpeakerRegistrationFailedException(string message, Exception innerException) : base(message, innerException) { }
        }
    }
}
