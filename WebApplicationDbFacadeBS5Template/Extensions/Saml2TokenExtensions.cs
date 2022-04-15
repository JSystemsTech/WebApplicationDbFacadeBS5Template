﻿using Microsoft.IdentityModel.Tokens.Saml2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationDbFacadeBS5Template.Services.Configuration;

namespace WebApplicationDbFacadeBS5Template.Extensions
{
    public class TokenClaim
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public IEnumerable<string> Values { get; set; }
        public TokenClaim() { }
        internal TokenClaim(string name, IEnumerable<string> values)
        {
            Name = name;
            Values = values.Count() == 1 ? values.First().Split(',') : values;
            Value = string.Join(",", Values);
        }
        internal IEnumerable<string> GetValues() => Values != null ? Values : !string.IsNullOrWhiteSpace(Value) ? Value.Split(',') : new string[0];
        internal string GetValue() => string.Join(",", GetValues());
    }
    public static class Saml2TokenExtensions
    {
        private static Saml2SecurityTokenHandler SamlTokenHandler = new Saml2SecurityTokenHandler();
        private static IEnumerable<TokenClaim> DefaultClaims = new TokenClaim[0];

        public static string CreateToken(IEnumerable<TokenClaim> claims)
            => Serialize(CreateSaml2SecurityToken(claims));
        public static DateTime? GetTokenExpirationDate(this string tokenStr)
            => Deserialize(tokenStr) is Saml2SecurityToken saml2Token && IsValidToken(saml2Token) ? saml2Token.Assertion.Conditions.NotOnOrAfter : null;
        public static bool TryGetValidTokenClaims(this string tokenStr, out IEnumerable<TokenClaim> claims)
        {
            if(Deserialize(tokenStr) is Saml2SecurityToken saml2Token && IsValidToken(saml2Token))
            {
                claims = GetClaims(saml2Token);
                return true;
            }
            claims = DefaultClaims;
            return false;
        }



        private static Saml2SecurityToken CreateSaml2SecurityToken(IEnumerable<TokenClaim> claims)
        {
            Saml2SubjectConfirmationData confirmationData = new Saml2SubjectConfirmationData() { Address = Saml2TokenConfiguration.ConfirmationMethod };
            Saml2SubjectConfirmation subjectConfirmations = new Saml2SubjectConfirmation(new Uri(Saml2TokenConfiguration.ConfirmationMethod), confirmationData);
            Saml2AudienceRestriction[] audienceRestriction = new Saml2AudienceRestriction[1] { new Saml2AudienceRestriction(Saml2TokenConfiguration.AudienceUri.ToString()) };
            Saml2Assertion assertion = new Saml2Assertion(new Saml2NameIdentifier(Saml2TokenConfiguration.Issuer))
            {
                Conditions = new Saml2Conditions(audienceRestriction)
                {
                    NotBefore = null,
                    NotOnOrAfter = null
                },
                InclusiveNamespacesPrefixList = Saml2TokenConfiguration.Namespace,
                Subject = new Saml2Subject(subjectConfirmations)
                {
                    NameId = new Saml2NameIdentifier(Saml2TokenConfiguration.ConfirmationMethod)
                }
            };


            return RenewToken(new Saml2SecurityToken(assertion), claims);
        }

        private static bool IsValidToken(Saml2SecurityToken saml2Token)
        {
            DateTime utcNow = DateTime.UtcNow;
            bool isValidIssueDate = saml2Token.Assertion.Conditions.NotBefore is DateTime issueDate && issueDate <= utcNow;
            bool isValidExpirationDate = saml2Token.Assertion.Conditions.NotOnOrAfter is DateTime expirationDate && expirationDate > utcNow;

            return isValidIssueDate && isValidExpirationDate && Saml2TokenConfiguration.Issuer == saml2Token.Assertion.Issuer.Value;
        }
        private static IEnumerable<TokenClaim> GetClaims(Saml2SecurityToken saml2Token)
            => saml2Token.Assertion.Statements.First() is Saml2AttributeStatement saml2AttributeStatement ?
            saml2AttributeStatement.Attributes.Select(a => new TokenClaim(a.Name, a.Values)) : DefaultClaims;



        private static Saml2SecurityToken RenewToken(Saml2SecurityToken saml2Token, IEnumerable<TokenClaim> claims)
        {
            int validForFinal = Saml2TokenConfiguration.ValidFor;
            DateTime utcNow = DateTime.UtcNow;
            saml2Token.Assertion.Conditions.NotBefore = utcNow;
            if (claims.FirstOrDefault(c=> c.Name == TokenClaimConfiguration.ExpireDateClaim) is TokenClaim expireDateClaim
                && DateTime.TryParse(expireDateClaim.Value, out DateTime expireDate))
            {
                saml2Token.Assertion.Conditions.NotOnOrAfter = expireDate.ToUniversalTime();
            }
            else
            {
                saml2Token.Assertion.Conditions.NotOnOrAfter = utcNow.AddMinutes(validForFinal);
            }

            foreach (TokenClaim claim in claims ?? DefaultClaims)
            {
                AddUpdateSaml2Attribute(saml2Token, claim.Name, claim.GetValue());
            }
            return saml2Token;
        }

        private static string Serialize(Saml2SecurityToken token)
        {
            var sw = new System.IO.StringWriter();
            using (var xmlWriter = new System.Xml.XmlTextWriter(sw))
            {
                SamlTokenHandler.WriteToken(xmlWriter, token);
                return sw.ToString().Encrypt();
            }

        }
        private static Saml2SecurityToken Deserialize(string tokenStr)
        {
            string decryptedValue = tokenStr.Decrypt();
            return SamlTokenHandler.CanReadToken(decryptedValue) ? SamlTokenHandler.ReadSaml2Token(decryptedValue) : null;
        }

        private static void AddUpdateSaml2Attribute(Saml2SecurityToken saml2Token, string name, object value)
        {
            if (!(saml2Token.Assertion.Statements.FirstOrDefault() is Saml2AttributeStatement))
            {
                saml2Token.Assertion.Statements.Add(new Saml2AttributeStatement());
            }
            if(saml2Token.Assertion.Statements.First() is Saml2AttributeStatement saml2AttributeStatement)
            {
                saml2AttributeStatement.Attributes.Remove(saml2AttributeStatement.Attributes.FirstOrDefault(saml2Attribute => saml2Attribute.Name == name));
                Saml2Attribute saml2AttributeReplacement = value is IEnumerable<string> stringEnumerableValue ? new Saml2Attribute(name, stringEnumerableValue) : new Saml2Attribute(name, value.ToString());
                saml2AttributeStatement.Attributes.Add(saml2AttributeReplacement);
            }            
        }
    }
}