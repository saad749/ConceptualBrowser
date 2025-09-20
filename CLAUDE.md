# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

ConceptualBrowser is a .NET-based text analysis application that implements Formal Concept Analysis (FCA) to extract and rank formal concepts from text. The solution consists of three projects: a business logic library, a Windows Forms UI, and a REST API.

## Build Commands

```bash
# Build the entire solution
cd ConceptualBrowser
dotnet build ConceptualBrowser.sln

# Build with Release configuration
dotnet build ConceptualBrowser.sln --configuration Release

# Clean build artifacts
dotnet clean ConceptualBrowser.sln
```

**Important**: The Business project requires the NTextCat.dll reference from ConceptualBrowser.FormUI\Assets. The build may show warnings about this missing reference, but the main functionality uses the newer Catalyst library for language detection.

## Running the Applications

```bash
# Run the Windows Forms application
cd ConceptualBrowser/ConceptualBrowser.FormUI
dotnet run

# Run the API
cd ConceptualBrowser/ConceptualBrowser.API
dotnet run
# API will be available at: https://localhost:[port]/swagger
```

## Project Structure

### ConceptualBrowser.Business (Core Library)
- **ConceptExtraction.cs**: Main algorithm orchestrator for formal concept analysis
- **Entities/**: Domain models (BinaryRelation, Coverage, OptimalConcept)
- **Common/Stemmers/**: 34+ language-specific stemming algorithms (Snowball-based)
- **Common/EmptyWords/**: Stop words for 27+ languages (embedded as resources)
- **LanguageDetection.cs**: Language detection using Catalyst/CLD2

### ConceptualBrowser.FormUI (Desktop App)
- Windows Forms application targeting .NET 6.0-windows
- Interactive UI for viewing extracted concepts, sentences, and keywords
- Contains NTextCat.dll in Assets folder (legacy dependency)

### ConceptualBrowser.API (Web API)
- ASP.NET Core Web API targeting .NET 7.0
- Single endpoint: `POST /api/Summarize` - accepts JSON-escaped text
- Returns concept-based summary with performance metrics
- Swagger documentation available at `/swagger`

## Key Architecture Patterns

### Language Processing Pipeline
1. **Language Detection**: Automatic detection using Catalyst CLD2
2. **Text Preprocessing**: Sentence splitting, tokenization
3. **Stemming**: Language-specific word normalization
4. **Stop Word Removal**: Using embedded language-specific empty words
5. **Concept Extraction**: FCA-based algorithm with configurable coverage thresholds

### Multi-Language Support
- Supports 27+ languages with dedicated stemmers and stop words
- Language codes follow ISO 639 standards
- Stemmer classes generated from Snowball algorithms
- Empty words stored as embedded resources in Common/EmptyWords/

### API Usage
The API expects JSON-escaped text input. Example:
```json
{
  "text": "Your JSON-escaped text here"
}
```

## Development Notes

### Framework Versions (Updated)
- All projects upgraded to **.NET 9.0** (latest LTS)
- Business & FormUI: .NET 9.0 / .NET 9.0-windows
- API: .NET 9.0
- Successfully upgraded from legacy .NET 6.0/7.0 versions

### Package Versions (Updated)
- **Catalyst**: 1.0.47753 (latest)
- **Iso639**: 1.0.0
- **Microsoft.Windows.Compatibility**: 9.0.0 (for FormUI)
- **Microsoft.AspNetCore.Mvc.NewtonsoftJson**: 9.0.0 (for API)
- **Swashbuckle.AspNetCore**: 7.2.0 (latest)

### Missing Test Infrastructure
Currently no test projects exist. When adding tests:
- Create ConceptualBrowser.Business.Tests for algorithm testing
- Create ConceptualBrowser.API.Tests for integration testing
- Use xUnit or NUnit as the test framework

### Performance Considerations
- ConceptExtraction includes timing and performance monitoring
- Coverage thresholds are configurable for scalability
- Binary relation matrix construction is memory-intensive for large texts

### Known Issues
- NTextCat.dll reference: Located in ConceptualBrowser.FormUI\Assets (legacy dependency, primary functionality uses Catalyst)
- Windows Forms warnings: CA1416 warnings for Windows-specific APIs are expected and safe to ignore for Windows Forms apps
- Designer serialization: Custom properties use DesignerSerializationVisibility.Hidden attribute to prevent designer issues