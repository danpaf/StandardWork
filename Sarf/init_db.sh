#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "standard" --dbname "standard" <<-EOSQL
    CREATE USER sarf_slave WITH PASSWORD 'sarf';
    CREATE DATABASE standard_sarf WITH OWNER 'sarf_slave';
EOSQL

psql -v ON_ERROR_STOP=1 --username "sarf_slave" --dbname "standard_sarf" <<-EOSQL
    CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
EOSQL