# Lab 7: Serialization and Data Persistence

## Description
This update implements data persistence, allowing the application to save the state of the object collection to a secondary storage and retrieve it later.

## Objectives
* Implement Serialization and Deserialization mechanisms.
* Support multiple file formats (`.csv` and `.json`).
* Handle file I/O operations and data parsing errors.

## Key Features
* **Multi-Format Storage:**
    * **JSON:** Implementation of standard JSON serialization/deserialization.
    * **CSV:** Custom parsing logic to read/write Comma-Separated Values, including handling of malformed or "broken" rows.
* **Menu Expansion:**
    * Save collection to file
    * Load collection from file
    * Clear current collection
* **Testing:** Expanded Unit Tests to verify file operations and ensure robust data parsing during deserialization.
