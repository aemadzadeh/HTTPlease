using HTTPlease.Tests.Mocks;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HTTPlease.Formatters.Tests
{
	using Formatters.Json;

	/// <summary>
	///		Tests for HTTP requests using content formatters.
	/// </summary>
	public class FormattedRequestTests
	{
		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative URI, expecting a JSON response, and then perform an HTTP POST request.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> representing asynchronous test execution.
		/// </returns>
		[Fact]
		public async Task Can_Build_Request_RelativeUri_ExpectJson_Post()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			MockMessageHandler mockHandler = new MockMessageHandler(
				request =>
				{
					Assert.NotNull(request);
					Assert.Equal(HttpMethod.Post, request.Method);
					Assert.Equal(
						new Uri(baseUri, "foo/bar"),
						request.RequestUri
					);

					Assert.Equal(1, request.Headers.Accept.Count);
					Assert.Equal(
						"application/json",
						request.Headers.Accept.First().MediaType
					);

					return request.CreateResponse(
						HttpStatusCode.OK,
						"Success!",
						"application/json",
						new JsonFormatter()
					);
				}
			);

			using (HttpClient mockClient = new HttpClient(mockHandler))
			{
				HttpResponseMessage response = await
					mockClient.PostAsync(
						HttpRequest.Create(baseUri)
							.WithRelativeRequestUri("foo/bar")
							.ExpectJson(),
						postBody: new StringContent("{}")
					);

				using (response)
				{
					Assert.True(response.IsSuccessStatusCode);
					Assert.NotNull(response.Content?.Headers?.ContentType);
					Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

					string responseBody = await response.Content.ReadAsStringAsync();
					Assert.Equal("\"Success!\"", responseBody);
				}
			}
		}

		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative URI, and then perform a JSON-formatted HTTP POST request.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> representing asynchronous test execution.
		/// </returns>
		[Fact]
		public async Task Can_Build_Request_RelativeUri_PostAsJson()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			MockMessageHandler mockHandler = new MockMessageHandler(
				async request =>
				{
					Assert.NotNull(request);
					Assert.Equal(HttpMethod.Post, request.Method);
					Assert.Equal(
						new Uri(baseUri, "foo/bar"),
						request.RequestUri
					);
					Assert.Equal(1, request.Headers.Accept.Count);
					Assert.Equal(
						"application/json",
						request.Headers.Accept.First().MediaType
					);

					string requestBody = await request.Content.ReadAsStringAsync();
					Assert.Equal("\"1234\"", requestBody);

					return request.CreateResponse(HttpStatusCode.OK,
						body: Int32.Parse(
							requestBody.Replace("\"", String.Empty)
						),
						mediaType: "application/json",
						formatter: new JsonFormatter()
					);
				}
			);

			using (HttpClient mockClient = new HttpClient(mockHandler))
			{
				int responseBody = await
					mockClient.PostAsJsonAsync(
						HttpRequest.Create(baseUri)
							.WithRelativeRequestUri("foo/bar")
							.UseJson()
							.ExpectJson(),
						postBody: "1234"
					)
					.ReadAsAsync<int>();

				Assert.Equal(1234, responseBody);
			}
		}
	}
}