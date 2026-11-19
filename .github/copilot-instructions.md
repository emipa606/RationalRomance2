# GitHub Copilot Instructions for Rational Romance 2 (Continued)

## Mod Overview and Purpose
Rational Romance 2 (Continued) is a RimWorld mod that extends and updates the original Rational Romance mod by adding various features and improvements related to romantic behavior in the game. This mod aims to provide a more nuanced and customizable approach to relationships within the colony, including sexual orientation, new traits, cultural influences, and polyamory.

## Key Features and Systems
### Orientations
- Adds sexual orientation traits (asexual, bisexual, gay, straight) to pawns.
- Configurable percentages for each orientation trait through the mod's options menu.

### New Traits
- Introduces "Faithful" and "Philanderer" traits to influence pawn behavior regarding cheating.

### New Activities
- Introduces "long walks" as a date activity and "casual hookups" as a joy activity.

### Improved Romantic Behavior
- Pawns will not attempt romance if recently rebuffed or if their lover has high opinion.
- Pawns will rebuff or breakup with partners of the "wrong" gender and refuse marriage proposals from them.
- Reduces the impact of the Ugly trait on romantic attraction.

### Cultural Influence
- Adds cultural components to romantic interactions based on pawn background.

### Polyamory
- Supports polyamorous pawns who prefer relationships involving multiple partners.

### Adjusted Mood Penalties
- Reduces penalties and stack limits for rebuffs and other romantic failures.

### Bug Fixes
- Corrects issues in vanilla game's romance-related code, such as gender balance in initiation and parentage.

## Coding Patterns and Conventions
- **Naming Conventions**: Use descriptive and consistent names for classes, methods, and variables.
- **Comments and Documentation**: Provide clear comments and documentation within the code to explain complex logic and decisions.
- **Error Handling**: Implement robust error handling to ensure the mod's stability and reliability.
- **Modularity**: Organize code into modular components to improve maintainability and extensibility.

## XML Integration
- **Defs**: Use XML files to define new game elements such as interaction definitions, job definitions, joy definitions, need definitions, and relations.
- **Structure**: Ensure XML files are well-structured and adhere to the RimWorld XML schema.
- **Validation**: Regularly validate XML files to catch potential errors and inconsistencies.

## Harmony Patching
- **Harmony**: Utilize Harmony patching to modify and extend vanilla game behavior without altering the original codebase.
- **Patch Naming**: Clearly name patch classes and methods to indicate their purpose and the original method they are patching.
- **Transpilers**: Use transpilers judiciously to modify method code for more complex behavior changes.
- **Prefixes and Postfixes**: Implement prefix and postfix methods to add or modify behavior before or after the original method execution.

## Suggestions for Copilot
- **Feature Requests**: Provide specific feature requests or areas where you would like Copilot to assist in improving the mod.
- **Code Examples**: Share code examples or snippets of existing mod code to help Copilot understand the context and coding patterns used.
- **Feedback**: Provide feedback on the quality and relevance of Copilot's suggestions to help improve future recommendations.
- **Documentation**: Request Copilot to generate or update documentation for new features or code changes.

By following these guidelines, you can ensure a more consistent and maintainable mod development process, leveraging Copilot to enhance your coding efficiency and creativity.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

