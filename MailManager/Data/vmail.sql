CREATE DATABASE vmail CHARACTER SET 'utf8';

GRANT INSERT, UPDATE, DELETE, SELECT ON vmail.* TO 'vmail'@'localhost' IDENTIFIED BY 'vmaildbpass';

USE vmail;

CREATE TABLE domains (
    id int unsigned NOT NULL AUTO_INCREMENT,
    domain varchar(255) NOT NULL,
    PRIMARY KEY (id),
    UNIQUE KEY (domain)
);

CREATE TABLE accounts (
    id int unsigned NOT NULL AUTO_INCREMENT,
    username varchar(64) NOT NULL,
    domain varchar(255) NOT NULL,
    password varchar(255) NOT NULL,
    quota int unsigned DEFAULT '0',
    enabled boolean DEFAULT '0',
    sendonly boolean DEFAULT '0',
    PRIMARY KEY (id),
    UNIQUE KEY (username, domain),
    FOREIGN KEY (domain) REFERENCES domains (domain)
);

CREATE TABLE aliases (
    id int unsigned NOT NULL AUTO_INCREMENT,
    source_username varchar(64) NOT NULL,
    source_domain varchar(255) NOT NULL,
    destination_username varchar(64) NOT NULL,
    destination_domain varchar(255) NOT NULL,
    enabled boolean DEFAULT '0',
    PRIMARY KEY (id),
    UNIQUE KEY (source_username, source_domain, destination_username, destination_domain),
    FOREIGN KEY (source_domain) REFERENCES domains (domain)
);

CREATE TABLE tlspolicies (
    id int unsigned NOT NULL AUTO_INCREMENT,
    domain varchar(255) NOT NULL,
    policy enum('none', 'may', 'encrypt', 'dane', 'dane-only', 'fingerprint', 'verify', 'secure') NOT NULL,
    params varchar(255),
    PRIMARY KEY (id),
    UNIQUE KEY (domain)
);

INSERT INTO domains (domain) VALUES ('bihalu.de');

-- Kennwort ist geheim ;-)
INSERT INTO accounts (username, domain, password, quota, enabled, sendonly) VALUES ('hansi', 'bihalu.de', '{SHA512-CRYPT}$6$3k/GqaYZ2V.Bky6.$xT1C1ZEG6l/BYZbBNCpoORWn6imAQPK8QsNd7Thr.1Ntg1ZinBmgen3bAzC25jsEdLKfwy07VQ28Bj1kJIPZ71', 2048, true, false);

