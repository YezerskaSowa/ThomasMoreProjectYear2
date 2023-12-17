using Contentful.Core;
using Contentful.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configure Contentful client
var contentfulOptions = builder.Configuration.GetSection("Contentful").Get<ContentfulOptions>();
if (contentfulOptions == null)
{
    throw new InvalidOperationException("Contentful configuration is missing in appsettings.json");
}

if (string.IsNullOrEmpty(contentfulOptions.DeliveryApiKey) || 
    string.IsNullOrEmpty(contentfulOptions.SpaceId))
{
    throw new InvalidOperationException("Contentful API Key and Space ID must be provided in appsettings.json");
}

var httpClient = new HttpClient();
// Instantiate ContentfulClient with the necessary parameters
var contentfulClient = new ContentfulClient(httpClient, contentfulOptions.DeliveryApiKey, contentfulOptions.SpaceId);
builder.Services.AddSingleton<IContentfulClient>(contentfulClient);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();




