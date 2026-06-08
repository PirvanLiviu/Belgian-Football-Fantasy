CREATE TABLE player_stats (
    id SERIAL PRIMARY KEY,
    player_id INT REFERENCES players(id) ON DELETE CASCADE,
    match_id INT REFERENCES matches(id) ON DELETE CASCADE,
    goals INT NOT NULL DEFAULT 0,
    assists INT NOT NULL DEFAULT 0,
    tackles_pct NUMERIC(5, 2) DEFAULT 0.00, -- Represents 'tackles (%)'
    passes_pct NUMERIC(5, 2) DEFAULT 0.00,-- Represents 'passes (%)'
    points INT DEFAULT 0
);

-- ==========================================
-- 1. PRIMARY / INDEPENDENT TABLES
-- ==========================================

-- Teams Table
CREATE TABLE teams (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    stadium VARCHAR(150),
    coach VARCHAR(100),
    badge VARCHAR(255) -- URL or file path to team logo
);

-- Users Table
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL, -- Stored as a hashed string
    budget NUMERIC(10, 2) NOT NULL DEFAULT 100.00,
    points INT NOT NULL DEFAULT 0,
    verified BOOLEAN NOT NULL DEFAULT FALSE,
    formation VARCHAR(5) NOT NULL DEFAULT '4-3-3'
);

-- Leagues Table
CREATE TABLE leagues (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    code VARCHAR(50) UNIQUE NOT NULL
);

-- ==========================================
-- 2. DEPENDENT TABLES (Level 1)
-- ==========================================

-- Players Table (Depends on teams)
CREATE TABLE players (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    team INT REFERENCES teams(id) ON DELETE SET NULL, -- Matches 'team' column in ERD
    position VARCHAR(50) NOT NULL,
    price NUMERIC(10, 2) NOT NULL,
    face VARCHAR(255), -- URL or file path to player image
    nationality VARCHAR(100)
);

-- Matches Table (Depends on teams)
CREATE TABLE matches (
    id SERIAL PRIMARY KEY,
    home_team INT REFERENCES teams(id) ON DELETE CASCADE,
    away_team INT REFERENCES teams(id) ON DELETE CASCADE,
    score VARCHAR(20), -- e.g., '2-1'
    gameweek INT NOT NULL,
    date TIMESTAMP NOT NULL
);

-- User League Junction Table (Depends on users, leagues)
CREATE TABLE user_league (
    id SERIAL PRIMARY KEY,
    user_id INT REFERENCES users(id) ON DELETE CASCADE,
    league_id INT REFERENCES leagues(id) ON DELETE CASCADE,
    CONSTRAINT unique_user_league UNIQUE (user_id, league_id)
);

-- Gameweeks Table (Depends on users)
CREATE TABLE gameweeks (
    id SERIAL PRIMARY KEY,
    gameweek INT NOT NULL,
    user_id INT REFERENCES users(id) ON DELETE CASCADE,
    points INT NOT NULL DEFAULT 0
);

-- ==========================================
-- 3. DEPENDENT TABLES (Level 2 / Junctions)
-- ==========================================

-- User Player Junction Table (Depends on users, players)
CREATE TABLE user_player (
    user_id INT REFERENCES users(id) ON DELETE CASCADE,
    player_id INT REFERENCES players(id) ON DELETE CASCADE,
    captain BOOLEAN NOT NULL DEFAULT FALSE,
    starting BOOLEAN NOT NULL DEFAULT TRUE,
    
    -- DEFINE THE COMPOSITE PRIMARY KEY HERE
    PRIMARY KEY (user_id, player_id)
);

-- Player Stats Table (Depends on players, matches)
CREATE TABLE player_stats (
    player_id INT REFERENCES players(id) ON DELETE CASCADE,
    match_id INT REFERENCES matches(id) ON DELETE CASCADE,
    goals INT NOT NULL DEFAULT 0,
    assists INT NOT NULL DEFAULT 0,
    tackles_pct NUMERIC(5, 2) DEFAULT 0.00, -- Represents 'tackles (%)'
    passes_pct NUMERIC(5, 2) DEFAULT 0.00,  -- Represents 'passes (%)'
    points INT DEFAULT 0,
    
    -- DEFINE THE COMPOSITE PRIMARY KEY HERE
    PRIMARY KEY (player_id, match_id)
);

-- ==========================================
-- 4. RECOMMENDED INDEXES FOR PERFORMANCE
-- ==========================================
CREATE INDEX idx_players_team ON players(team);
CREATE INDEX idx_matches_gameweek ON matches(gameweek);
CREATE INDEX idx_player_stats_lookup ON player_stats(player_id, match_id);
CREATE INDEX idx_user_player_lookup ON user_player(user_id, player_id);
