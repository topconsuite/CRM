using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Infra.Integrations.DTOs.AssinaturaEletronica.Clicksign.Request;
using TopSys.TopConWeb.Infra.Integrations.DTOs.AssinaturaEletronica.Clicksign.Response;
using TopSys.TopConWeb.Infra.Integrations.Helpers;
using TopSys.TopConWeb.Infra.Integrations.RestClients;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Infra.Integrations.Services
{
    public class ClicksignIntegrationService : IAssinaturaEletronicaIntegrationService
    {
        private readonly ClicksignRestClient _clicksignRestClient;
        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        public ClicksignIntegrationService(ClicksignRestClient clicksignRestClient)
        {
            _clicksignRestClient = clicksignRestClient;
        }

        public void ConfigurarContextoUsina(int? usinaId)
        {
            _clicksignRestClient.SetUsinaContext(usinaId);
        }

        public ClicksignSigner AddSignerToDocument(ClicksignDocument clicksignDocument, ClicksignSigner clicksignSigner, bool assinaturaContratada = false, bool assinaturaVendedor = false, bool assinaturaTestemunha = false)
        {
            var request = RequestBuilder.Build(Method.POST, "/api/v1/lists");
            var addSignerToDocumentRequest = "";

            if (assinaturaContratada) {
                addSignerToDocumentRequest = JsonConvert.SerializeObject(new AddSignerHiredToDocumentRequest { List = new AddSignerHiredToDocumentDTO(clicksignDocument.Id, clicksignSigner.Id) }, jsonSerializerSettings);
            } else if (assinaturaVendedor)
            {
                addSignerToDocumentRequest = JsonConvert.SerializeObject(new AddSignerSellerToDocumentRequest { List = new AddSignerSellerToDocumentDTO(clicksignDocument.Id, clicksignSigner.Id) }, jsonSerializerSettings);
            } else if (assinaturaTestemunha) 
            {
                addSignerToDocumentRequest = JsonConvert.SerializeObject(new AddSignerWitnessToDocumentRequest { List = new AddSignerWitnessToDocumentDTO(clicksignDocument.Id, clicksignSigner.Id) }, jsonSerializerSettings);
            } else
            {
                addSignerToDocumentRequest = JsonConvert.SerializeObject(new AddSignerToDocumentRequest { List = new AddSignerToDocumentDTO(clicksignDocument.Id, clicksignSigner.Id) }, jsonSerializerSettings);
            }

            request.AddJsonBody(addSignerToDocumentRequest);
            
            var response = _clicksignRestClient.Execute(request);

            if (!response.IsSuccessful)
            {
                var resultError = JsonConvert.DeserializeObject<ClicksignErrorResponse>(response.Content);
                foreach (var error in resultError.Erros)
                    AssertionConcern.Notify("clicksign", error);

                return null;
            }

            var result = JsonConvert.DeserializeObject<AddSignerToDocumentResponse>(response.Content);

            clicksignSigner.RequestSignatureKey = result.Signer.RequestSignatureKey;

            return clicksignSigner;
        }

        public ClicksignSigner AddSignerCoResponsibleToDocument(ClicksignDocument clicksignDocument, ClicksignSigner clicksignSigner)
        {
            var request = RequestBuilder.Build(Method.POST, "/api/v1/lists");
            var addSignerToDocumentRequest = "";

            addSignerToDocumentRequest = JsonConvert.SerializeObject(new AddSignerCoResponsibleToDocumentRequest { List = new AddSignerCoResponsibleToDocumentDTO(clicksignDocument.Id, clicksignSigner.Id) }, jsonSerializerSettings);

            request.AddJsonBody(addSignerToDocumentRequest);

            var response = _clicksignRestClient.Execute(request);

            if (!response.IsSuccessful)
            {
                var resultError = JsonConvert.DeserializeObject<ClicksignErrorResponse>(response.Content);
                foreach (var error in resultError.Erros)
                    AssertionConcern.Notify("clicksign", error);

                return null;
            }

            var result = JsonConvert.DeserializeObject<AddSignerToDocumentResponse>(response.Content);

            clicksignSigner.RequestSignatureKey = result.Signer.RequestSignatureKey;

            return clicksignSigner;
        }

        public ClicksignDocument CreateDocument(ClicksignDocument clicksignDocument)
        {
            var request = RequestBuilder.Build(Method.POST, "/api/v1/documents");

            var createDocumentRequest = JsonConvert.SerializeObject(new CreateDocumentRequest { Document = new DTOs.AssinaturaEletronica.Clicksign.Request.DocumentDTO(clicksignDocument.Path, clicksignDocument.File, clicksignDocument.Deadline) }, jsonSerializerSettings);

            request.AddJsonBody(createDocumentRequest);
            var response = _clicksignRestClient.Execute(request);

            if (!response.IsSuccessful)
            {
                var resultError = JsonConvert.DeserializeObject<ClicksignErrorResponse>(response.Content);
                foreach (var error in resultError.Erros)
                    AssertionConcern.Notify("clicksign", error);

                return null;
            }

            var result = JsonConvert.DeserializeObject<DocumentResponse>(response.Content);

            clicksignDocument.Id = result.Document.Id;

            return clicksignDocument;
        }

        public ClicksignSigner CreateSigner(ClicksignSigner clicksignSigner)
        {
            var request = RequestBuilder.Build(Method.POST, "/api/v1/signers");

            var createSigner = JsonConvert.SerializeObject(new CreateSignerRequest { Signer = new DTOs.AssinaturaEletronica.Clicksign.Request.SignerDTO(clicksignSigner) }, jsonSerializerSettings);

            request.AddJsonBody(createSigner);
            var response = _clicksignRestClient.Execute(request);

            if (!response.IsSuccessful)
            {
                var resultError = JsonConvert.DeserializeObject<ClicksignErrorResponse>(response.Content);
                foreach (var error in resultError.Erros)
                    AssertionConcern.Notify("clicksign", $"{error}");

                return null;
            }
               
            var result = JsonConvert.DeserializeObject<SignerResponse>(response.Content);

            clicksignSigner.Id = result.Signer.Id;

            return clicksignSigner;
        }

        public ClicksignSigner CreateSignerHired(ClicksignSigner clicksignSignerHired)
        {
            var request = RequestBuilder.Build(Method.POST, "/api/v1/signers");

            var createSignerHired = JsonConvert.SerializeObject(new CreateSignerHiredRequest { Signer = new DTOs.AssinaturaEletronica.Clicksign.Request.SignerHiredDTO(clicksignSignerHired) }, jsonSerializerSettings);

            request.AddJsonBody(createSignerHired);
            var response = _clicksignRestClient.Execute(request);

            if (!response.IsSuccessful)
            {
                var resultError = JsonConvert.DeserializeObject<ClicksignErrorResponse>(response.Content);
                foreach (var error in resultError.Erros)
                    AssertionConcern.Notify("clicksign", $"{error}");

                return null;
            }

            var result = JsonConvert.DeserializeObject<SignerResponse>(response.Content);

            clicksignSignerHired.Id = result.Signer.Id;

            return clicksignSignerHired;
        }

        public ClicksignSigner CreateSignerCoResponsible(ClicksignSigner clicksignSignerCoResponsible)
        {
            var request = RequestBuilder.Build(Method.POST, "/api/v1/signers");

            var createSignerCoResponsible = JsonConvert.SerializeObject(new CreateSignerCoResponsibleRequest { Signer = new DTOs.AssinaturaEletronica.Clicksign.Request.SignerCoResponsibleDTO(clicksignSignerCoResponsible) }, jsonSerializerSettings);

            request.AddJsonBody(createSignerCoResponsible);
            var response = _clicksignRestClient.Execute(request);

            if (!response.IsSuccessful)
            {
                var resultError = JsonConvert.DeserializeObject<ClicksignErrorResponse>(response.Content);
                foreach (var error in resultError.Erros)
                    AssertionConcern.Notify("clicksign", $"{error}");

                return null;
            }

            var result = JsonConvert.DeserializeObject<SignerResponse>(response.Content);

            clicksignSignerCoResponsible.Id = result.Signer.Id;

            return clicksignSignerCoResponsible;
        }

        public byte[] DownloadFile(string FileUrl)
        {
            if (FileUrl == "") return null;
            
            var client = new RestClient(FileUrl);
            var request = new RestRequest();

            return client.DownloadData(request);
        }

        public string GetClicksignUrlDocumentSigned(Guid documentId)
        {
            var request = RequestBuilder.Build(Method.GET, $"api/v1/documents/{documentId}");

            var response = _clicksignRestClient.Execute(request);

            if (response.StatusCode == HttpStatusCode.NotFound) return "";

            if (!response.IsSuccessful)
                throw new Exception(response.ErrorMessage, response.ErrorException);

            var result = JsonConvert.DeserializeObject<DocumentResponse>(response.Content);

            return result.Document.DownloadDocument.SignedFileUrl;
        }

        public void RequestDocumentCancelClicksign(Guid documentId)
        {
            var request = RequestBuilder.Build(Method.PATCH, $"/api/v1/documents/{documentId}/cancel");

            var response = _clicksignRestClient.Execute(request);

            if (!response.IsSuccessful)
            {
                var resultError = JsonConvert.DeserializeObject<ClicksignErrorResponse>(response.Content);
                foreach (var error in resultError.Erros)
                    AssertionConcern.Notify("clicksign", $"{error}");
            }

            return;
        }

        public void RequestSignaturesEmail(ClicksignSigner clicksignSigner)
        {
            var request = RequestBuilder.Build(Method.POST, "/api/v1/notifications");

            var signerRequest = JsonConvert.SerializeObject(new RequestSignatureRequest(clicksignSigner.RequestSignatureKey, clicksignSigner?.MessageEmail ?? ""), jsonSerializerSettings);

            request.AddJsonBody(signerRequest);

            var response = _clicksignRestClient.Execute(request);

            if (!response.IsSuccessful)
                throw new Exception(response.ErrorMessage, response.ErrorException);

            return;
        }

        public void RequestSignaturesWhatsApp(ClicksignSigner clicksignSigner)
        {
            var request = RequestBuilder.Build(Method.POST, "/api/v1/notify_by_whatsapp");

            var signerRequest = JsonConvert.SerializeObject(new RequestSignatureRequest(clicksignSigner.RequestSignatureKey), jsonSerializerSettings);

            request.AddJsonBody(signerRequest);

            var response = _clicksignRestClient.Execute(request);

            if (!response.IsSuccessful)
                throw new Exception(response.ErrorMessage, response.ErrorException);

            return;
        }
    }
}
